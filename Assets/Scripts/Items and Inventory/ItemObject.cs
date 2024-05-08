using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    private void OnValidate()
    {
        if (itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object "+itemData.name;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            Debug.Log("Pick up "+itemData.name);
            Inventory.instance.AddItem(itemData);
            Destroy(gameObject);
        }
    }
}
