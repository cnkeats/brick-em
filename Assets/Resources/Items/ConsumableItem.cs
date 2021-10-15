using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ConsumableItem : InventoryItem
{
    public ConsumableItem() : base()
    {
        type = ItemType.CONSUMABLE;
    }

    private void Awake()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(delegate { Consume(); });
    }

    public virtual void Consume()
    {
        Debug.Log(string.Format("Consumed {0}", name));
        Destroy(gameObject);

        inventoryManager.Remove(this);
    }
}
