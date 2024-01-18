using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemHolder : MonoBehaviour
{
    [SerializeField] protected Item currentItem;
    [SerializeField] protected CanvasGroup slotCG;
    [SerializeField] protected Image itemImg;
    [SerializeField] protected TextMeshProUGUI itemAmountText;
    public Action OnSetItemToSlot;
    public Item CurrentItem { get { return currentItem; }}
    public void SetItemToSlot(Item item)
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
        OnSetItemToSlot?.Invoke();
    }
}
