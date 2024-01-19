using UnityEngine.EventSystems;

public class InventorySlot : InteractableSlot
{




    public override void OnPointerDown(PointerEventData eventData)
    {
        if (currentItem != null)
        {
            base.OnPointerDown(eventData);
   
        }
    }

    protected override void SetItem()
    {
        base.SetItem();
        OnStopDrag?.Invoke(slotIndex);
        inventorySystem.CursorFollowingItem.SetItemToSlot(null);
    }
}
