using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Pool;

public class InventorySystem : MonoBehaviour
{
    private Inventory inventory;
    [SerializeField] private InventoryUI inventoryUI;
    [SerializeField] SceneItem sceneItemPrefab;
    public SceneItemsPool sceneItemsPool;

    private void Awake()
    {
        inventory = new Inventory();
        inventoryUI.SetInventory(inventory);


    }

    private void Start()
    {
        var sceneItem = sceneItemsPool.objectPool.Get();
        sceneItem.transform.position = new Vector3(2.03f, 0.1f, 0f);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out SceneItem sceneItem))
        {
            inventory.AddItem(sceneItem.Item);
            sceneItemsPool.objectPool.Release(sceneItem);
        }
    }

    public void ToggleInventory(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            inventoryUI.ToggleInventoryUI();
        }
    }

    internal void AddItem(Item item)
    {
        inventory.AddItem(item);
    }
}
