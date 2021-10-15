using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public int itemLimit = 32;

    public List<InventoryItem> items;

    private GameObject inventorySlots;

    private void OnEnable()
    {
        Refresh();
    }

    public void Refresh()
    {
        inventorySlots = transform.Find("InventoryPanel/InventorySlots").gameObject;
        items = new List<InventoryItem>();

        foreach (Transform transform in inventorySlots.transform)
        {
            InventoryItem item = transform.GetComponent<InventoryItem>();
            if (item == null)
            {
                Destroy(transform.gameObject);
            }
            else
            {
                items.Add(item);
            }
        }

        AddEmptySlots(itemLimit - items.Count);
    }

    public void Remove(InventoryItem item)
    {
        items.Remove(item);
        AddEmptySlots(1);
    }

    private void AddEmptySlots(int num)
    {
        for (int i = 0; i < num; i++)
        {
            GameObject emptySlot = Instantiate(Resources.Load("Items/InventorySlot")) as GameObject;
            emptySlot.name = string.Format("Empty Slot", i);
            emptySlot.transform.SetParent(inventorySlots.transform);
        }
    }
}
