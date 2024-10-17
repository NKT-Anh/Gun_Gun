using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class CollectableItem
{
    [Range(0f, 1f)]
    public float dropChance;
    public int mount;
    public Item LootItemPrefab;
}
public class ItemManager : Singleton<ItemManager>
{
    [SerializeField] private CollectableItem[] items;
    public void spawn(Vector3 spawnPos)
    {
        if(items == null || items.Length <=0)
        {
            return;
        }
        float spawnRate = Random.value;
        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            if (item == null || item.dropChance < spawnRate)continue;
            {
                CreateItem(item, spawnPos);
            }

        }
    }
   private void CreateItem(CollectableItem collectableItem, Vector3 spawnPostion)
    {
        if(collectableItem == null) return;
        for (int i = 0;i< collectableItem.mount;i++)
        {
            Instantiate(collectableItem.LootItemPrefab, spawnPostion, Quaternion.identity);
        }

    }
}
