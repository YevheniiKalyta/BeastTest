using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class InventorySystem : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private int inventoryCapacity = 8;
    [SerializeField] CraftingSystem craftingSystem;
    [SerializeField] private InventoryUI inventoryUI;
    public SceneItemsPool sceneItemsPool;
    [SerializeField] private CursorFollowingItem cursorFollowingItem;
    public CursorFollowingItem CursorFollowingItem { get { return cursorFollowingItem; } }
    public InventoryUI InventoryUI { get { return inventoryUI; } }

    private void Awake()
    {
        inventory = new Inventory(inventoryCapacity, craftingSystem.GetCraftingSlotCount() + 1);
        inventoryUI.SetInventory(inventory);
    }

    public void TryDropItem()
    {
        var sceneItem = sceneItemsPool.objectPool.Get();
        sceneItem.SetItem(new Item(cursorFollowingItem.CurrentItem));
        cursorFollowingItem.SetItemToSlot(null);
        Vector3 sceneItemPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        sceneItem.transform.position = sceneItemPos;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SceneItem sceneItem) && sceneItem.readyToPickUp)
        {
            if (inventory.AddItem(sceneItem.Item))
            {
                sceneItemsPool.objectPool.Release(sceneItem);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out SceneItem sceneItem) && !sceneItem.readyToPickUp)
        {
            sceneItem.readyToPickUp = true;
        }
    }

    public void ToggleInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            inventoryUI.ToggleInventoryUI();
        }
    }

    public void AddItem(Item item)
    {
        inventory.AddItem(item);
    }
    public void AddItemAtIndex(Item item, int index, bool force = false)
    {
        inventory.AddItemAtIndex(item, index, force);
    }
    public void RemoveItem(Item item)
    {
        inventory.RemoveItem(item);
    }
    public void RemoveItemAtIndex(Item item, int index)
    {
        inventory.RemoveItemFromSlot(item, index);
    }

    public void Swap(Item[] arr, int indexA, int indexB)
    {
        if (cursorFollowingItem.CurrentItem?.itemSO == arr[indexB]?.itemSO && cursorFollowingItem.CurrentItem?.itemSO != null && indexA != indexB)
        {
            arr[indexB].amount += cursorFollowingItem.CurrentItem.amount;
            arr[indexA] = null;
        }
        else
        {
            Item tmp = cursorFollowingItem.CurrentItem;
            arr[indexA] = arr[indexB];
            arr[indexB] = tmp;

        }
        inventory.OnInventoryChanged?.Invoke();
    }
}
