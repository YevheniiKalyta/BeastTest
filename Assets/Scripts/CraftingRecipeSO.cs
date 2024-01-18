using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewRecipe", menuName = "SO/CraftingRecipe")]
public class CraftingRecipeSO : ScriptableObject
{
    public ItemSO finalItem;
    public List<ItemSO> itemsToCombine;
    [Range(0f,100f)]
    public float successChance;
}
