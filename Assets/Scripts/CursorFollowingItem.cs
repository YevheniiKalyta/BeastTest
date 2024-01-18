using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CursorFollowingItem : ItemHolder
{

    public RectTransform parentRectTransform;
    public Item CurrentItem { get { return currentItem; } }
    private void Update()
    {
        UpdatePosition();
    }
    private void UpdatePosition()
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(parentRectTransform, Mouse.current.position.ReadValue(), null, out Vector2 localPoint);
        transform.localPosition = localPoint;
    }

    public void SetItem(Item item)
    {
        currentItem = item;

        if (currentItem != null)
        {
            slotCG.alpha = 1;
        }
        else
        {
            slotCG.alpha = 0;
            return;
        }

        itemImg.sprite = currentItem.itemSO.itemImg;
        itemAmountText.text = currentItem.amount.ToString();
    }
}
