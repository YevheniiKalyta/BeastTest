using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] List<CraftingRecipeSO> craftingRecipeSOs = new List<CraftingRecipeSO>();
    [SerializeField] List<CraftingSlot> itemHolders = new List<CraftingSlot>();
    [SerializeField] CraftingSlot finalItemSlot;
    [SerializeField] List<Image> craftingLines = new List<Image>();
    [SerializeField] float craftingSpeed;
    Sequence sequence;
    private CraftingRecipeSO currentRecipe;
    [SerializeField] InventorySystem inventorySystem;


    public int GetCraftingSlotCount() => itemHolders.Count;
    private void Start()
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].OnSetItemToSlot += CheckForFinalItem;
        }
        finalItemSlot.SetItemToSlot(finalItemSlot.CurrentItem);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].OnSetItemToSlot -= CheckForFinalItem;
        }
    }

    private void CheckForFinalItem()
    {
        if ((sequence == null || !sequence.IsPlaying()) && !removingItems)
        {

            currentRecipe = GetRecipe();
            if (currentRecipe != null)
            {
                if (finalItemSlot.CurrentItem == null || (finalItemSlot.CurrentItem.itemSO != null
                 && currentRecipe.finalItem.itemSO == finalItemSlot.CurrentItem.itemSO
                 && finalItemSlot.CurrentItem.itemSO.stackable))
                {
                    AnimateCrafting();
                }
            }
        }
    }

    public CraftingRecipeSO GetRecipe()
    {
        List<Item> itemsToCombine = itemHolders.Select(x => x.CurrentItem).ToList();
        for (int i = 0; i < craftingRecipeSOs.Count; i++)
        {
            if (CompareRecipeItems(craftingRecipeSOs[i]))
            {
                return craftingRecipeSOs[i];
            }
        }
        return null;
    }

    private bool CompareRecipeItems(CraftingRecipeSO craftingRecipeSO)
    {
        List<Item> slotItems = itemHolders.Select(x => x.CurrentItem).ToList();
        for (int i = 0; i < craftingRecipeSO.itemsToCombine.Count; i++)
        {
            Item craftingItem = craftingRecipeSO.itemsToCombine[i];
            bool hasItem = false;
            for (int j = 0; j < slotItems.Count; j++)
            {
                if (slotItems[j] != null && slotItems[j].itemSO == craftingItem.itemSO && slotItems[j].amount >= craftingItem.amount)
                {
                    slotItems.RemoveAt(j);
                    hasItem = true;
                    break;
                }
            }
            if (!hasItem)
            {
                return false;
            }
        }

        return true;
    }

    private void AnimateCrafting()
    {
        sequence = DOTween.Sequence();
        for (int i = 0; i < craftingLines.Count; i++)
        {

            sequence.Join(craftingLines[i].DOFillAmount(1f, craftingSpeed));
        }
        sequence.onKill = () =>
        {
            for (int i = 0; i < craftingLines.Count; i++)
            {
                craftingLines[i].fillAmount = 0f;
            }
        };
        sequence.OnComplete(() => TryToCraft());
        sequence.Play();
        SoundManager.Instance.PlaySFXOneShot("craftingProcess");
    }

    private void TryToCraft()
    {
        float chance = UnityEngine.Random.Range(0f, 100f);
        removingItems = true;
        if (chance <= currentRecipe.successChance)
        {

            inventorySystem.AddItemAtIndex(new Item(currentRecipe.finalItem), finalItemSlot.SlotIndex);
            RemoveItems();
            SoundManager.Instance.PlaySFXOneShot("craftGood");
        }
        else
        {
            RemoveItems();
            finalItemSlot.GetComponent<Image>().DOColor(Color.red, 0.5f).OnComplete(() => { finalItemSlot.GetComponent<Image>().DOColor(Color.white, 0.5f); });
            SoundManager.Instance.PlaySFXOneShot("craftBad");
        }
        CheckForFinalItem();
    }
    bool removingItems = false;
    private void RemoveItems()
    {
        removingItems = true;
        List<Item> itemsToCombine = itemHolders.Select(x => x.CurrentItem).ToList();
        for (int i = 0; i < currentRecipe.itemsToCombine.Count; i++)
        {
            Item craftingItem = currentRecipe.itemsToCombine[i];
            for (int j = 0; j < itemsToCombine.Count; j++)
            {
                if (itemsToCombine[j] != null && itemsToCombine[j].itemSO == craftingItem.itemSO)
                {
                    inventorySystem.RemoveItemAtIndex(craftingItem, itemHolders[j].SlotIndex);
                    itemsToCombine[j] = null;
                    break;
                }
            }
        }
        removingItems = false;
    }

    //private void UpdateCraftingSlots()
    //{
    //    for (int i = 0; i < itemHolders.Count; i++)
    //    {
    //        itemHolders[i].SetItemToSlot(itemHolders[i].CurrentItem);
    //    }
    //}
}
