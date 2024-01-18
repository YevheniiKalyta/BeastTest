using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] List<InventorySlot> slots = new List<InventorySlot>();
    [SerializeField] CursorFollowingItem cursorFollowingItem;
    public Dictionary<InventorySlot, Item> itemsDisplayed = new Dictionary<InventorySlot, Item>();
    private int startSlot;

    private void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetIndex(i);
        }
        InventorySlot.OnStartDrag += OnStartItemDrag;
        InventorySlot.OnStopDrag += OnStopItemDrag;
    }

    private void OnStopItemDrag(int endSlot)
    {
        if (endSlot != -1)
        {
            if (startSlot != -1)
            {
                inventory.Swap(inventory.Items, startSlot, endSlot);

            }
            else
            {
                inventory.AddItemAtIndex(cursorFollowingItem.CurrentItem, endSlot);
            }
        }
        startSlot = -1;
    }

    private void OnStartItemDrag(int slotIndex)
    {
        startSlot = slotIndex;
    }

    public void SetInventory(Inventory inventoryToSet)
    {
        inventory = inventoryToSet;
        inventory.OnInventoryChanged += RefreshInventory;
        InitializeInventory();
    }

    internal void ToggleInventoryUI()
    {

    }

    private void RefreshInventory()
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            slots[i].SetItemToSlot(inventory.Items[i]);
        }
    }

    public void InitializeInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.Items.Length)
            {
                slots[i].SetItemToSlot(inventory.Items[i]);
                itemsDisplayed.Add(slots[i], inventory.Items[i]);
            }
            else
            {
                slots[i].SetItemToSlot(null);
                itemsDisplayed.Add(slots[i], null);
            }
        }
    }
}
