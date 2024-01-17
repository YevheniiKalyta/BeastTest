using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] List<InventorySlot> slots = new List<InventorySlot>();


    public void SetInventory(Inventory inventoryToSet)
    {
        inventory = inventoryToSet;
        inventory.OnInventoryChanged += RefreshInventory;
        RefreshInventory();
    }

    internal void ToggleInventoryUI()
    {

    }

    private void RefreshInventory()
    {


        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.Items.Count)
            {
                slots[i].SetItemToSlot(inventory.Items[i]);
            }
            else
            {
                slots[i].SetItemToSlot(null);
            }
        }


        for (int i = 0; i < inventory.Items.Count; i++)
        {
            if (i < slots.Count)
            {
                slots[i].SetItemToSlot(inventory.Items[i]);
            }
            else Debug.LogError("Number of slots < number of inventory items");
        }
    }
}
