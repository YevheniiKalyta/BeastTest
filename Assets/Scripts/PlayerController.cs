using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    [SerializeField] float  playerSpeed;
    [SerializeField] PlayerInputActions inputActions;
    [SerializeField] Animator animator;
    void Awake()
    {
        inputActions = new PlayerInputActions();
        inputActions.DefaultMap.Enable();
    }

    void FixedUpdate()
    {
        Vector2 input = inputActions.DefaultMap.Movement.ReadValue<Vector2>();
        Vector3 motionVector = new Vector3(input.x, 0, input.y);

        rb.AddForce(motionVector * playerSpeed, ForceMode.Force);
        animator.SetFloat("Horizontal", input.x);
        animator.SetFloat("Vertical", input.y);
        animator.SetFloat("Speed", motionVector.sqrMagnitude);
    }
}
