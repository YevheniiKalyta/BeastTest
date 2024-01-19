using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGiver : MonoBehaviour, IInteractable
{
    [SerializeField] ItemSO itemSO;
    [SerializeField] ParticleSystem particleSystem;
    [SerializeField] Material material;

    public void Interact(PlayerController playerController)
    {
        Debug.Log($"interactdd {gameObject.name}");
        playerController.InventorySystem.AddItem(new Item(itemSO, 1));
        particleSystem.Play();
        material = new Material( material );
        material.mainTexture = itemSO.itemImg.texture;
        particleSystem.GetComponent<ParticleSystemRenderer>().material = material;

    }
}
