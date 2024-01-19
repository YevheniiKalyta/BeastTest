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
                    SetItemToSlot(currentItem);
                }
            }
            else
            {
                SetItemToSlot(inventorySystem.CursorFollowingItem.CurrentItem);
            }
            OnStopDrag?.Invoke(slotIndex);
        }
        else
        {
            OnCancelDrag?.Invoke(slotIndex);
        }
        inventorySystem.CursorFollowingItem.SetItemToSlot(null);
    }

    public override void SetItemToSlot(Item item)
    {
        base.SetItemToSlot(item);
        if (canBeFilled)
        {
            OnSetItemToSlot?.Invoke();
        }
    }
}
