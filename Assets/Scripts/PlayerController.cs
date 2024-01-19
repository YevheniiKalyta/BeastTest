using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float  playerSpeed;
    [SerializeField] PlayerInputActions inputActions;
    [SerializeField] Animator animator;
    [SerializeField] float interactionRadius;
    [SerializeField] LayerMask interactionLayer;
    [SerializeField] InstructionsUI instructionsUI;
    [SerializeField] SphereCollider triggerZone;
    [SerializeField] InventorySystem inventorySystem;
    bool footstepsPlaying;
    public InventorySystem InventorySystem { get { return inventorySystem; } }
    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.DefaultMap.Enable();
        triggerZone.radius = interactionRadius;
    }

    void FixedUpdate()
    {
        Vector2 input = inputActions.DefaultMap.Movement.ReadValue<Vector2>();
        Vector3 motionVector = new Vector3(input.x, 0, input.y);

        rb.AddForce(motionVector * playerSpeed, ForceMode.Force);
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Speed", motionVector.sqrMagnitude);
        if(motionVector.sqrMagnitude > 0)
        {
            if (!footstepsPlaying)
            {
                SoundManager.Instance.PlaySFXLoop("footsteps");
                footstepsPlaying = true;
            }
        }
        else if(footstepsPlaying)
        {
            SoundManager.Instance.StopSXFLoop("footsteps");
            footstepsPlaying = false;
        }
    }

    public void OnInteractionButtonPressed(InputAction.CallbackContext callbackContext)
    {
        if (callbackContext.performed)
        {
            var colliders = Physics.OverlapSphere(transform.position, interactionRadius, interactionLayer);
            if (colliders.Length > 0)
            {
                colliders = colliders.OrderBy(x => Vector3.Distance(transform.position, x.transform.position)).ToArray();
                var interactionObject = colliders[0].gameObject;
                if (interactionObject.TryGetComponent(out IInteractable interactable))
                {
                    interactable.Interact(this);
                }
            }
        }

    }

    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            instructionsUI.SetInstruction(InstructionsType.Interaction);
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent(out IInteractable interactable))
        {
            instructionsUI.SetInstruction(InstructionsType.None);
        }
    }


#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1f, 0.92f, 0.016f, 0.2f);
        Gizmos.DrawSphere(transform.position, interactionRadius);
    }
#endif
}
