using Cinemachine;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

public class InventoryUI : MonoBehaviour
{
    Inventory inventory;
    [SerializeField] InventorySystem inventorySystem;
    [SerializeField] List<InteractableSlot> slots = new List<InteractableSlot>();
    [SerializeField] CursorFollowingItem cursorFollowingItem;
    [SerializeField] RectTransform mainPanel;
    [SerializeField] Canvas canvas;
    [SerializeField] float animationTime = 1f;
    [SerializeField] CinemachineVirtualCamera virtualCamera;
    bool uiOn;
    Tween tween;
    private int startSlot;
    public int StartSlot { get { return startSlot; } }
    public int GetSlotCount() { return slots.Count; }
    private void Start()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            slots[i].SetIndex(i);
        }
        InventorySlot.OnStartDrag += OnStartItemDrag;
        InventorySlot.OnStopDrag += OnStopItemDrag;
        InventorySlot.OnCancelDrag += OnCancelDrag;
    }



    private void OnCancelDrag(int endSlot)
    {
        if (startSlot != -1)
        {
            inventorySystem.Swap(inventory.Items, startSlot, startSlot);
        }
        else
        {
            inventory.AddItem(cursorFollowingItem.CurrentItem);
        }
    }

    private void OnStopItemDrag(int endSlot)
    {
        if (endSlot != -1)
        {
            if (startSlot != -1)
            {
                if (inventory.Items[startSlot]?.itemSO == null && cursorFollowingItem.CurrentItem?.itemSO != inventory.Items[endSlot]?.itemSO)
                {
                    inventorySystem.Swap(inventory.Items, startSlot, endSlot);
                }
                else
                {
                    inventory.AddItemAtIndex(cursorFollowingItem.CurrentItem, endSlot);
                }

                startSlot = -1;
            }
        }
    }

    private void OnStartItemDrag(int slotIndex)
    {
        startSlot = slotIndex;
    }

    public void SetInventory(Inventory inventoryToSet)
    {
        inventory = inventoryToSet;
        inventory.OnInventoryChanged += RefreshInventory;
        InitializeInventory();
    }

    internal void ToggleInventoryUI()
    {
        uiOn = !uiOn;
        if (tween != null)
        {
            tween.Kill();
        }
        if (uiOn)
        {

            tween = mainPanel.DOAnchorPos(new Vector2(Screen.width / 4, 0f), animationTime).OnStart(() => canvas.enabled = true).SetEase(Ease.OutBack);
            DOTween.To(() => virtualCamera.m_Lens.LensShift.x, x => virtualCamera.m_Lens.LensShift.x = x, 0.25f, animationTime);
        }
        else
        {
            tween = mainPanel.DOAnchorPos(new Vector2(Screen.width, 0f), animationTime).OnComplete(() => canvas.enabled = false).SetEase(Ease.OutBack);
            DOTween.To(() => virtualCamera.m_Lens.LensShift.x, x => virtualCamera.m_Lens.LensShift.x = x, 0f, animationTime);
        }
        tween.easeOvershootOrAmplitude = 0.5f;
    }

    private void RefreshInventory()
    {
        for (int i = 0; i < inventory.Items.Length; i++)
        {
            slots[i].SetItemToSlot(inventory.Items[i]);
        }
    }

    public void InitializeInventory()
    {
        for (int i = 0; i < slots.Count; i++)
        {
            if (i < inventory.Items.Length)
            {
                slots[i].SetItemToSlot(inventory.Items[i]);
            }
            else
            {
                slots[i].SetItemToSlot(null);
            }
        }
    }
}
