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
    private void Start()
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].OnSetItemToSlot += CheckForFinalItem;
        }
        UpdateCraftingSlots();
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
        if (sequence == null || !sequence.IsPlaying())
        {
            currentRecipe = GetRecipe();
            if (currentRecipe != null)
            {
                AnimateCrafting();
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
    }

    private void TryToCraft()
    {
        float chance = UnityEngine.Random.Range(0f, 1f);
        if (chance <= currentRecipe.successChance)
        {
            finalItemSlot.AddItemToSlot(new Item(currentRecipe.finalItem.itemSO, currentRecipe.finalItem.amount));
            RemoveItems();
            UpdateCraftingSlots();
        }
        else
        {
            RemoveItems();
            finalItemSlot.GetComponent<Image>().DOColor(Color.red, 0.5f).OnComplete(() => { finalItemSlot.GetComponent<Image>().DOColor(Color.white, 0.5f); });
            UpdateCraftingSlots();
        }
    }

    private void RemoveItems()
    {
        List<Item> itemsToCombine = itemHolders.Select(x => x.CurrentItem).ToList();
        for (int i = 0; i < currentRecipe.itemsToCombine.Count; i++)
        {
            Item craftingItem = currentRecipe.itemsToCombine[i];
            for (int j = 0; j < itemsToCombine.Count; j++)
            {
                if (itemsToCombine[j] != null && itemsToCombine[j].itemSO == craftingItem.itemSO)
                {
                    itemHolders[j].CurrentItem.amount -= craftingItem.amount;

                    itemsToCombine[j] = null;
                    break;
                }
            }
        }
    }

    private void UpdateCraftingSlots()
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].SetItemToSlot(itemHolders[i].CurrentItem);
        }
    }

    public void CreateFinalItem(ItemSO finalItem)
    {

    }
}
