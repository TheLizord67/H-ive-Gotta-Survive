using UnityEngine;
using System;

public class playerController : MonoBehaviour
{
    public enum InputMode { Keyboard, Controller }
    private InputMode currentInputMode;
    private InputMode lastInputMode;
    public static Action<InputMode> OnInputModeChanged;

    [SerializeField] private float basePlayerSpeed;
    [SerializeField] private float currentPlayerSpeed;
    [SerializeField] private float rotationRate; //horizontal sens
    [SerializeField] private float vertRotateRate; //vert sens
    [SerializeField] private float gravMult;
    [SerializeField] private float jumpForce;
    [SerializeField] private float controllerSensMult = 1.5f;
    private float curRotationRate; //horizontal sens
    private float curVertRotateRate; //vert sens
    private Transform playerCamera;
    private float vertConstraints; //constraints for vert camera looking
    private Rigidbody rb;
    private float sprintMod = 2;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        curVertRotateRate = vertRotateRate;
        curRotationRate = rotationRate;
        currentInputMode = InputMode.Keyboard;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = Camera.main.transform;
    }

    // Update is called once per frame
    void Update()
    {
        currentInputMode = ProcessInputMode();
        if (currentInputMode != lastInputMode)
        {
            OnInputModeChanged?.Invoke(currentInputMode);
        }
        lastInputMode = ProcessInputMode();

        if (currentInputMode == InputMode.Controller)
        {
            curRotationRate = rotationRate * controllerSensMult;
            curVertRotateRate = vertRotateRate * controllerSensMult;
        }
        else
        {
            curRotationRate = rotationRate;
            curVertRotateRate = vertRotateRate;
        }

        Debug.Log(currentInputMode);
        //gravity
        rb.AddForce(Vector3.down * gravMult);
        //player movement
        Vector3 playerVelocity = gameObject.transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * currentPlayerSpeed;
        rb.linearVelocity = playerVelocity;

        if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.JoystickButton8))
        {
            currentPlayerSpeed = currentPlayerSpeed * sprintMod;
        }
        else if (Input.GetKeyUp(KeyCode.LeftShift) || Input.GetKeyUp(KeyCode.JoystickButton8))
        {
            currentPlayerSpeed = basePlayerSpeed;
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }

        //camera
        gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * curRotationRate);//horizontal
        vertConstraints = Mathf.Clamp(Input.GetAxis("Mouse Y") * -curVertRotateRate + vertConstraints, -60f, 60f);
        playerCamera.localRotation = Quaternion.Euler(new Vector3(vertConstraints, 0f, 0f)); //probably vert
    }
    private InputMode ProcessInputMode()
    {
        //if (Input.GetJoystickNames().Length == 0)
        //{
        //    return InputMode.Keyboard;

        //}
        if (Input.anyKeyDown)
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton0)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton1)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton2)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton3)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton4)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton5)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton6)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton7)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton8)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton9)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton10)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton11)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton12)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton13)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton14)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton15)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton16)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton17)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton18)) return InputMode.Controller;
            else if (Input.GetKeyDown(KeyCode.JoystickButton19)) return InputMode.Controller;
            else return InputMode.Keyboard;
        }
        return currentInputMode;
    }
}