#region

using UnityEngine;

#endregion

namespace LethalBestiary.Extras;

[CreateAssetMenu(menuName = "ScriptableObjects/SpawnableOutsideObject")]
public class SpawnableOutsideObjectDef : ScriptableObject
{
    public SpawnableOutsideObjectWithRarity spawnableMapObject;
}