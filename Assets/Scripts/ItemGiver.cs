using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO itemSO;

    public void Interact(PlayerController playerController)
    {
        Debug.Log($"interactdd {gameObject.name}");
        playerController.InventorySystem.AddItem(new Item(itemSO, 1));
    }
}
