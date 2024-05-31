using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    [SerializeField] private ItemData itemData;

    [SerializeField] private Rigidbody2D rb;
    //private Vector2 velocityDrop;
    private bool canAddForce=true;
    private float impossiblePickUpTimer;
    private bool canPickUp;
    private void OnValidate()
    {
        setupVisuals();
    }

    private void setupVisuals()
    {
        if (itemData == null) return;
        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item Object " + itemData.name;
    }

    public void SetUpDrop(ItemData _item, Vector2 _velocity, float duration)
    {
        itemData = _item;
        rb.velocity = _velocity;
        impossiblePickUpTimer = duration;
        setupVisuals();
    }

    public void PickUpItem()
    {
        if (!canPickUp) return;

        if (!Inventory.instance.CanAddItem() && itemData.itemType == ItemType.Equipment)
        {
            if (canAddForce)
                StartCoroutine(AddForce());
            return;
        }
        //Debug.Log("Pick up " + itemData.name);
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }

    private void Update()
    {
        if (canPickUp) return;

        impossiblePickUpTimer -= Time.deltaTime;
        if (impossiblePickUpTimer < 0)
        {
            canPickUp = true;
        }
    }

    private IEnumerator AddForce()
    {
        canAddForce = false;
        rb.velocity = new Vector2(0, 5);
        yield return new WaitForSeconds(0.4f);
        canAddForce = true;
    }
}
