using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    [SerializeField] List<CraftingRecipeSO> craftingRecipeSOs = new List<CraftingRecipeSO>();
    [SerializeField] List<CraftingSlot> itemHolders = new List<CraftingSlot>();

    private void Start()
    {
        for (int i = 0; i < itemHolders.Count; i++)
        {
            itemHolders[i].OnSetItemToSlot += CheckForFinalItem;
        }
    }

    private void CheckForFinalItem()
    {
        if(GetFinalItem()!=null)
        {
            Debug.LogError("IT CAN BE SMTHNG");
        }
    }

    public ItemSO GetFinalItem()
    {
        List<ItemSO> itemSOsToCombine = itemHolders.Select(x=>x.CurrentItem?.itemSO).ToList();
        for (int i = 0; i < craftingRecipeSOs.Count; i++)
        {
            var diff1 = craftingRecipeSOs[i].itemsToCombine.Except(itemSOsToCombine).ToList();
            var diff2 = itemSOsToCombine.Except(craftingRecipeSOs[i].itemsToCombine).ToList();
            if(!diff1.Any() && !diff2.Any())
            {
                return craftingRecipeSOs[i].finalItem;
            }
        }
        return null;
    }



    public void CreateFinalItem(ItemSO finalItem)
    {

    }
}
