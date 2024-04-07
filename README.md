# Xilef992-LethalBestiary-LC
This is a lighweight API to load enemy which relies on [Lethal Lib](https://thunderstore.io/c/lethal-company/p/Evaisa/LethalLib/),
I do not appropriate myself any of the methods and classes given from this API, they belong to their original creator and are only seperated from the main code in the objective of making a better supported API for monster implementation.
Right now this API only helps you adding monster and doesn't provide any functions, but in the future, I will be adding a bunch of different methods that are going to be useful to create monsters.
This API is meant to be lightweight making it less likely to break with future updates
Join me on my [Discord](https://discord.gg/FqrRjuv7Xu) if you have any questions! Or ask them on the LethalCompany Modding Server
## Usage :
### Adding as dependency :
First, you will need to add LethalBestiary as a dependecy of your solution. The way to do it is different for both VisualStudio and rider so follow those documentations for those to IDE :

[Visual Studio](https://learn.microsoft.com/en-us/visualstudio/ide/how-to-create-and-remove-project-dependencies?view=vs-2022)

For **Rider**, simply right click your solutions dependency, click on add dependecy. On the popup window, click on add From and then reference LethalBestiary.dll

### CAREFUL STEP : REMOVING UNSUSED DEPENDENCY
This is very important, if you are using LethalLib or LethalLevelLoader and you use other stuff of those dependencies, it will be important to make sure you are referencing the RIGHT dependency for our 3 api uses the same naming convention.
If you only want to add enemy, use this api, if you want to add more stuff than enemy, I'd suggest using Lethal Level Loader once it updates or the current LethalLib.

### Imports
In your Plugin.cs, add the following Imports :
```c#
using LethalBestiary.Modules;
```
### Loading order
It will be important for this API to load before your mod else it will break, so you will have to add the following line above your PluginClass so that it will __always__ load before yours!
Here is an example :
```c#
namespace NightmareFoxyLC {
    //Plugin BepInEx declaration
    [BepInPlugin(ModGUID, PluginInfo.PLUGIN_NAME, PluginInfo.PLUGIN_VERSION)]
    //LINE TO ADD! ModGUID is a reference your mod GUID
    [BepInDependency(LethalBestiary.Plugin.ModGUID)]

    public class Plugin : BaseUnityPlugin {
        //PluginOther stuff
```
### Implementing your enemy!
Now to the main dishes, Here is how you implement your enemy into the game registry!
In your Awake() function of your Plugin.cs, the following must be present :

```c#
InitializeNetworkBehaviours();
NetworkPrefabs.RegisterNetworkPrefab(FoxyEnemy.enemyPrefab);
Enemies.RegisterEnemy(EnemyType, RarityInt, Levels.LevelTypes.All, Enemies.SpawnType.Default, EnemyTerminalNodeTN, EnemyTerminalKeywordTk);
```
What are those you may ask? [Let me break it down for you!](https://www.youtube.com/watch?v=dQw4w9WgXcQ)

#### __InitializeNetworkBehaviours();__
This must be called before we register the next step. Don't give it any parameters, leave it as is!
#### __NetworkPrefabs.RegisterNetworkPrefab(EnemyType.enemyPrefab);__
We register our prefab on the network so that there will be good syncing, missing this step will result in your enemy not despawning for some players, the enemy killing people who don't see him and worse.
What I am giving to this method is a reference to my enemyPrefab connected to my enemy type.
As you may know, I got my enemy type from
``var EnemyType = ModAssetsBundle.LoadAsset<EnemyType>("NameOfEemyTypeAssetInAssetBundle");``
And this EnemyType holds a reference to my prefab!
#### __Enemies.RegisterEnemy(EnemyType, RarityInt, Levels.LevelTypes.All, Enemies.SpawnType.Default, EnemyTerminalNodeTN, EnemyTerminalKeywordTk);__
Here we are at the most important step! **Registering our enemy!** FINALY!!! Ok, so here are the parameters :
- EnemyType : This is your enemy type you got from your Asset Bundle
- RarityInt : This is how rare your enemy is! This is multiplied by your enemy Spawning logarithm you made inside unity
- Levels.LevelTypes.All : This is which moon your enemy can spawn. it is an enum included with the library and you can choose a specific moon ``Levels.LevelTypes.AssuranceLevel``, modded moons only : ``Levels.LevelTypes.Modded``, vanilla moons only : ``Levels.LevelTypes.Vanilla`` or all moons as I did earlier ``Levels.LevelTypes.All``
- Enemies.SpawnType.Default : This is where your enemy can spawn, it is an enum included with the library. Here are the three choices : Inside the faculty : ``Enemies.SpawnType.Default``, outside : ``Enemies.SpawnType.Outside`` or like pacive monsters (outside but with a twist of not being considered an enemy) : ``Enemies.SpawnType.Daytime``
- TerminalNode : Reference to your AssetBundles terminal node which you got with ``ModAssets.LoadAsset<TerminalNode>("MonsterTN");`` You have to create it and add it to your bundle!
- TerminalKeyword : Reference to your AssetBundles terminal node which you got with ``ModAssets.LoadAsset<TerminalNode>("MonsterTK");`` You have to create it and add it to your bundle!\

### Playing with your enemy!
Your enemy is now in the game!
Have fun!
You will have to have this package as a dependency to your mod tho...


## Other useful function

The ``AddEnemyToLevel(SpawnableEnemy spawnableEnemy, SelectableLevel level)``
you can add your enemy to a specific level.
