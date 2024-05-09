using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private GameObject dropPrefab;
    [SerializeField] private int amountPossibleItemsDrop = 1;
    [SerializeField] private float impossiblePickupDuration=1;

    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    //[SerializeField] private ItemData itemDrop;

    public virtual void GenerateDrop()
    {
        for(int i = 0; i < possibleDrop.Length; i++)
        {
            if (Random.Range(0, 100) < possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        for(int i = 0;i < amountPossibleItemsDrop; i++)
        {
            if (dropList.Count == 0) return;
            ItemData newItemDrop = dropList[Random.Range(0,dropList.Count)];
            dropList.Remove(newItemDrop);

            DropItem(newItemDrop);
        }
    }

    protected virtual void DropItem(ItemData _item)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);
        Vector2 randomVelocityDrop = new Vector2(Random.Range(-7,7),Random.Range(10,15));
        newDrop.GetComponent<ItemObject>().SetUpDrop(_item, randomVelocityDrop, impossiblePickupDuration);

    }
}
