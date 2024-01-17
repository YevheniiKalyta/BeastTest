using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventorySlot : MonoBehaviour
{
    [SerializeField] private Item currentItem;
    [SerializeField] private Image itemImg;
    [SerializeField] private TextMeshProUGUI itemAmountText;
    [SerializeField] private CanvasGroup slotCG;

    public void SetItemToSlot(Item item)
    {
        currentItem = item;

        if(currentItem != null)
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
