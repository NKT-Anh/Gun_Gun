using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemLoot" , menuName = "Stats/ItemLoot") ]
public class LootItem : ScriptableObject
{
    public GameObject itemPrefab;
    public float dropChance;

}
