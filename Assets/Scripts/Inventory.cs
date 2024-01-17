using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory
{
    private List<Item> items = new List<Item>();

    public List<Item> Items { get { return items; } }

    public Action OnInventoryChanged;

    public Inventory()
    {
        items = new List<Item>();
    }

    public void AddItem(Item item)
    {
        if(item.itemSO.stackable)
        {
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].itemSO == item.itemSO)
                {
                    items[i].amount += item.amount;
                    OnInventoryChanged?.Invoke();
                    return;
                }
            }
            items.Add(item);
        }
        else
        {
            items.Add(item);
        }
        OnInventoryChanged?.Invoke();
    }
}
