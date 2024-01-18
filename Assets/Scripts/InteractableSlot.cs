using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableSlot : ItemHolder, IDropHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected InventorySystem inventorySystem;
    public static Action<int> OnStartDrag;
    public static Action<int> OnStopDrag;
    public void OnDrop(PointerEventData eventData)
    {
        SetItem();
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            inventorySystem.CursorFollowingItem.SetItem(new Item(currentItem));
            SetItemToSlot(null);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.hovered.Contains(this.gameObject))
        {
            SetItem();
        }

        var isOnInventorySlot = eventData.hovered.Any(x => x.TryGetComponent(out InteractableSlot slot));
        if (!isOnInventorySlot)
        {
            inventorySystem.TryDropItem();
        }
    }

    protected virtual void SetItem()
    {

    }

}
