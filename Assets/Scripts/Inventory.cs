using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Inventory
{
    private Item[] items;

    public Item[] Items { get { return items; } }

    public Action OnInventoryChanged;

    public Inventory(int capacity)
    {
        items = new Item[capacity];
    }

    public void AddItem(Item item)
    {
        if (item.itemSO.stackable)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i]?.itemSO == item.itemSO)
                {
                    items[i].amount += item.amount;
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
            InsertInFirstEmptySlot(item);
        }
        else
        {
            InsertInFirstEmptySlot(item);
        }
        OnInventoryChanged?.Invoke();
    }


    public void AddItemAtIndex(Item item,int index, bool force = false)
    {
        if (items[index] == null || force == true)
        {
            items[index] = new Item(item);
        }
        else if (items[index].itemSO ==  item.itemSO)
        {
            items[index].amount += item.amount;
        }
        else
        {
            AddItem(item);
        }
        OnInventoryChanged?.Invoke();
    }

    private void InsertInFirstEmptySlot(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                break;
            }
        }
    }

    public void RemoveItem(Item item)
    {
        for (int i = 0; i < items.Length; i++)
        {
            if (items[i]?.itemSO == item.itemSO)
            {
                items[i].amount -= item.amount;
                if (items[i].amount <= 0)
                {
                    items[i] = null;
                }
                OnInventoryChanged?.Invoke();
                return;
            }
        }
    }

    public void RemoveItemFromSlot(Item item, int index)
    {

            if (items[index]?.itemSO == item.itemSO)
            {
                items[index].amount -= item.amount;
                if (items[index].amount <= 0)
                {
                    items[index] = null;
                }
                OnInventoryChanged?.Invoke();
            }
    }


}
