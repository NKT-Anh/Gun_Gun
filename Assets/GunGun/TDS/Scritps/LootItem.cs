using UnityEngine;

[CreateAssetMenu(fileName = "New Loot Item", menuName = "Loot Item")]
public class LootItem : ScriptableObject
{
    public GameObject itemPrefab;
    public float dropChance;
}
