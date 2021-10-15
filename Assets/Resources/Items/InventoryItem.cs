using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour
{
    public ItemType type;

    public string description;
    public string blurb;

    protected InventoryManager inventoryManager;
    protected PlayerController player;

    public enum ItemType
    {
        NONE,
        CONSUMABLE,
        PASSIVE
    }

    private void OnEnable()
    {
        inventoryManager = FindObjectOfType<InventoryManager>();
        player = FindObjectOfType<PlayerController>();
    }
}
