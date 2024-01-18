using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor.UI;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot : InteractableSlot
{

    private int slotIndex;


    public void SetIndex(int ind)
    {
        slotIndex = ind;
    }


    public override void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            base.OnPointerDown(eventData);
            OnStartDrag?.Invoke(slotIndex);
        }
    }

    protected override void SetItem()
    {
        base.SetItem();
        OnStopDrag?.Invoke(slotIndex);
        inventorySystem.CursorFollowingItem.SetItem(null);
    }
}
