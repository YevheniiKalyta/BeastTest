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
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] SceneItem sceneItemPrefab;
    public SceneItemsPool sceneItemsPool;
    [SerializeField] private CursorFollowingItem cursorFollowingItem;
    public CursorFollowingItem CursorFollowingItem { get { return cursorFollowingItem; } }

    private void Awake()
    {
        inventory = new Inventory(inventoryCapacity);
        inventoryUI.SetInventory(inventory);



    }

    private void Start()
    {
        var sceneItem = sceneItemsPool.objectPool.Get();
        sceneItem.transform.position = new Vector3(2.03f, 0.1f, 0f);
    }

    public void TryDropItem()
    {
        var sceneItem = sceneItemsPool.objectPool.Get();
        sceneItem.SetItem(new Item(cursorFollowingItem.CurrentItem));
        inventory.RemoveItem(cursorFollowingItem.CurrentItem);
        cursorFollowingItem.SetItem(null);
        Vector3 sceneItemPos = new Vector3(transform.position.x, 0.1f, transform.position.z);
        sceneItem.transform.position = sceneItemPos;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SceneItem sceneItem) && sceneItem.readyToPickUp)
        {
            inventory.AddItem(sceneItem.Item);
            sceneItemsPool.objectPool.Release(sceneItem);
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
    public void RemoveItem(Item item)
    {
        inventory.RemoveItem(item);
    }
}
