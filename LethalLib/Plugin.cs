#region

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Security.Permissions;
using BepInEx;
using BepInEx.Configuration;
using LethalBestiary.Modules;
using MonoMod.Cil;
using MonoMod.RuntimeDetour;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

#endregion

[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
namespace LethalBestiary;

[BepInPlugin(ModGUID, ModName, ModVersion)]
//[BepInDependency("LethalExpansion", BepInDependency.DependencyFlags.SoftDependency)]
public class Plugin : BaseUnityPlugin
{
    public const string ModGUID = "Xilef."+MyPluginInfo.PLUGIN_GUID;//MyPluginInfo.PLUGIN_GUID;
    public const string ModName = MyPluginInfo.PLUGIN_NAME;
    public const string ModVersion = MyPluginInfo.PLUGIN_VERSION;

    public static BepInEx.Logging.ManualLogSource logger;
    public static ConfigFile config;

    public static Plugin Instance;

    public static ConfigEntry<bool> extendedLogging;

    private void Awake()
    {
        Instance = this;
        config = Config;
        logger = Logger;

        Logger.LogInfo($"LethalBestiary Is loading ...");

        extendedLogging = Config.Bind("General", "ExtendedLogging", false, "Enable extended logging");

        new ILHook(typeof(StackTrace).GetMethod("AddFrames", BindingFlags.Instance | BindingFlags.NonPublic), IlHook);
        Enemies.Init();
        Utilities.Init();

        Logger.LogInfo($"LethalBestiary Is Loaded");

    }

    private void IlHook(ILContext il)
    {
        try
        {
            var cursor = new ILCursor(il);
            cursor.GotoNext(
                x => x.MatchCallvirt(typeof(StackFrame).GetMethod("GetFileLineNumber", BindingFlags.Instance | BindingFlags.Public))
            );
            cursor.RemoveRange(2);
            cursor.EmitDelegate<Func<StackFrame, string>>(GetLineOrIL);
        }
        catch (Exception e)
        {
            Logger.LogInfo("ILHook already applied!");
        }
    }

    private static string GetLineOrIL(StackFrame instance)
    {
        var line = instance.GetFileLineNumber();
        if (line == StackFrame.OFFSET_UNKNOWN || line == 0)
        {
            return "IL_" + instance.GetILOffset().ToString("X4");
        }

        return line.ToString();
    }
}
