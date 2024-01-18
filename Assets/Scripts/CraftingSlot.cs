using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CraftingSlot : InteractableSlot
{
    [SerializeField] bool canBeFilled;
    protected override void SetItem()
    {
        base.SetItem();
        if (canBeFilled)
        {
            if (currentItem?.itemSO != null)
            {
                if (inventorySystem.CursorFollowingItem.CurrentItem.itemSO == currentItem.itemSO)
                {
                    currentItem.amount += inventorySystem.CursorFollowingItem.CurrentItem.amount;
                    SetItemToSlot(currentItem);
                }
                else
                {
                    Item itemToSwap = currentItem;
                    SetItemToSlot(inventorySystem.CursorFollowingItem.CurrentItem);
                    inventorySystem.AddItemAtIndex(itemToSwap, inventorySystem.InventoryUI.StartSlot, true);
                }
            }
            else
            {
                SetItemToSlot(inventorySystem.CursorFollowingItem.CurrentItem);
            }
            OnStopDrag?.Invoke(-1);
        }
        else
        {
            OnCancelDrag?.Invoke(-1);
        }
        inventorySystem.CursorFollowingItem.SetItemToSlot(null);
    }
}
