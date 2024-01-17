using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneItem : MonoBehaviour
{
    private Item item;
    [SerializeField ] private SpriteRenderer itemImg;
    public Item Item { get { return item; }}

    public void SetItem(Item itemToSet)
    {
        item = itemToSet;
        itemImg.sprite = item.itemSO.itemImg;
    }
}
