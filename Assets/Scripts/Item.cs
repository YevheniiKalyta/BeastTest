using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item
{
    public ItemSO itemSO;
    public int amount;

    public Item(ItemSO itemSO, int amount)
    {
        this.itemSO = itemSO;
        this.amount = amount;
    }

    public Sprite GetItemImg()
    {
        return itemSO.itemImg;
    }

    public string GetName()
    {
        return itemSO.name;
    }

}
