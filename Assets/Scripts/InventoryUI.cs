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

    private int startSlot;
    public int StartSlot { get { return startSlot; } }

    private void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetIndex(i);
        }
        InventorySlot.OnStartDrag += OnStartItemDrag;
        InventorySlot.OnStopDrag += OnStopItemDrag;
        InventorySlot.OnCancelDrag += OnCancelDrag;
    }

    private void OnCancelDrag(int endSlot)
    {
        if (startSlot != -1)
        {
            inventory.Swap(inventory.Items, startSlot, startSlot);
        }
        else
        {
            inventory.AddItem(cursorFollowingItem.CurrentItem);
        }
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
        else
        {
            if (startSlot != -1)
            {
                inventory.RemoveItemFromSlot(cursorFollowingItem.CurrentItem, startSlot);
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
            }
            else
            {
                slots[i].SetItemToSlot(null);
            }
        }
    }
}
