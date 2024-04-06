#region

using UnityEngine;

#endregion

namespace LethalBestiary.Extras;

[CreateAssetMenu(menuName = "ScriptableObjects/SpawnableMapObject")]
public class SpawnableMapObjectDef : ScriptableObject
{
    public SpawnableMapObject spawnableMapObject;
}