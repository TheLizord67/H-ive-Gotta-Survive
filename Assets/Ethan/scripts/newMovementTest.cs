using System;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.InputSystem;
public class newMovementTest : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] private float currentPlayerSpeed = 10;
    [SerializeField] private float jumpHeight = 2f;
    [SerializeField] private float gravity = -9.8f;
    private CharacterController controller;
    private Vector3 moveInput;
    private Vector3 velocity;
    public InputActionReference Move, Look;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        rb = GetComponent<Rigidbody>();
    }
    public void OnMove(InputAction.CallbackContext context)
    { 
        moveInput = context.ReadValue<Vector3>();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(moveInput);
        //rb.AddForce(moveInput);
        //Vector3 playerVelocity = gameObject.transform.rotation * new Vector3(moveInput.x, 0f, moveInput.z) * currentPlayerSpeed;
        //rb.linearVelocity = playerVelocity; 
        /*camera
        gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * rotationRate);//horizontal
        vertConstraints = Mathf.Clamp(Input.GetAxis("Mouse Y") * -vertRotateRate + vertConstraints, -60f, 60f);
        playerCamera.localRotation = Quaternion.Euler(new Vector3(vertConstraints, 0f, 0f)); //probably vert
        */
    }
    public void OnLook(InputAction.CallbackContext context)
    {

    }
}
