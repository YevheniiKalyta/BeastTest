using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSlot : InteractableSlot
{
    protected override void SetItem()
    {
        base.SetItem();
        SetItemToSlot(inventorySystem.CursorFollowingItem.CurrentItem);
        inventorySystem.RemoveItem(inventorySystem.CursorFollowingItem.CurrentItem);
        OnStopDrag?.Invoke(-1);
        inventorySystem.CursorFollowingItem.SetItem(null);

    }
}
