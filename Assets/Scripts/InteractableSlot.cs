using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class InteractableSlot : ItemHolder, IDropHandler, IDragHandler, IPointerClickHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] protected InventorySystem inventorySystem;
    protected int slotIndex;
    public static Action<int> OnStartDrag;
    public static Action<int> OnStopDrag;
    public static Action<int> OnCancelDrag;
    public int SlotIndex { get { return slotIndex; } }

    public void SetIndex(int ind)
    {
        slotIndex = ind;
    }
    public void OnDrop(PointerEventData eventData)
    {
        SetItem();
        SoundManager.Instance.PlaySFXOneShot("buttonClickHi");
    }

    public void OnDrag(PointerEventData eventData)
    {

    }

    public void OnPointerClick(PointerEventData eventData)
    {

    }

    public virtual void OnPointerDown(PointerEventData eventData)
    {

        if (eventData.button == PointerEventData.InputButton.Left)
        {
            if (currentItem != null && currentItem.itemSO != null)
            {
                inventorySystem.CursorFollowingItem.SetItemToSlot(new Item(currentItem));
                inventorySystem.RemoveItemAtIndex(currentItem, slotIndex);
                SetItemToSlot(null);
            }
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            var pickedItem = new Item(currentItem.itemSO, 1);

            inventorySystem.RemoveItemAtIndex(pickedItem, slotIndex);
            inventorySystem.CursorFollowingItem.SetItemToSlot(pickedItem);
        }
        OnStartDrag?.Invoke(slotIndex);
        SoundManager.Instance.PlaySFXOneShot("buttonClick");

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
