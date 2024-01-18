using UnityEngine.EventSystems;

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
        inventorySystem.CursorFollowingItem.SetItemToSlot(null);
    }
}
