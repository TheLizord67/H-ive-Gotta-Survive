using UnityEngine;
using System;

public class playerController : MonoBehaviour
{
    public enum InputMode { Keyboard, Controller }
    [SerializeField] private InputMode currentInputMode;
    private InputMode lastInputMode;
    public static Action<InputMode> OnInputModeChanged;

    [SerializeField] private float basePlayerSpeed;
    [SerializeField] private float currentPlayerSpeed;
    [SerializeField] private float rotationRate; //horizontal sens
    [SerializeField] private float vertRotateRate; //vert sens
    [SerializeField] private float gravMult;
    [SerializeField] private float jumpForce;
    [SerializeField] private float controllerSensMult = 1.5f;
    private float speedSpeed;
    private bool controllerIsSprinting = false;
    private float curRotationRate; //horizontal sens
    private float curVertRotateRate; //vert sens
    private Transform playerCamera;
    private float vertConstraints; //constraints for vert camera looking
    private Rigidbody rb;
    private float sprintMod = 2;

    [Space(10)]
    [Header("Toby")]
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject mainCanvas;
    [SerializeField] private float interactRayLength;
    [SerializeField] private LayerMask mask;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCanvas = GameObject.FindGameObjectWithTag("Main Canvas");
        speedSpeed = gameObject.GetComponent<speedometer>().speed;
        curVertRotateRate = vertRotateRate;
        curRotationRate = rotationRate;
        currentInputMode = InputMode.Keyboard;
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        playerCamera = Camera.main.transform;
        inventory = Instantiate(inventory, mainCanvas.transform);
        RectTransform rectTransform = inventory.GetComponent<RectTransform>();
        rectTransform.anchoredPosition = Vector2.zero;
        rectTransform.localScale = Vector3.one;
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
        //gravity
        rb.AddForce(Vector3.down * gravMult);
        //player movement
        Vector3 playerVelocity = gameObject.transform.rotation * new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical")) * currentPlayerSpeed;
        rb.linearVelocity = playerVelocity;
        playerVelocity = playerVelocity.normalized;
        //sprint
        //controller sprint doesnt work, figure out later
        if (currentInputMode == InputMode.Keyboard && Input.GetKeyDown(KeyCode.LeftShift))
        {
            currentPlayerSpeed = currentPlayerSpeed * sprintMod;
        }
        else if ((currentInputMode == InputMode.Keyboard && Input.GetKeyUp(KeyCode.LeftShift)) || Input.GetKeyDown(KeyCode.JoystickButton8))
        {
            currentPlayerSpeed = basePlayerSpeed;
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton8) && currentInputMode == InputMode.Controller)
        {
            if (!controllerIsSprinting)
            {
                currentPlayerSpeed = currentPlayerSpeed * sprintMod;
                controllerIsSprinting = true;
            }
            if (controllerIsSprinting || speedSpeed == 0)
            {
                currentPlayerSpeed = basePlayerSpeed;
                controllerIsSprinting = false;
            }
        }
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.JoystickButton0))
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            Debug.Log("Jump");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton1) || Input.GetKeyDown(KeyCode.Q))
        {
            Debug.Log("Drop");
        }
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out hit, interactRayLength, mask))
        {
            if (Input.GetKeyDown(KeyCode.JoystickButton2) || Input.GetKeyDown(KeyCode.E))
            {
                Debug.Log("Interact");
            }
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton4) || Input.GetKeyDown(KeyCode.Alpha5))
        {
            Debug.Log("Left Utility");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton5) || Input.GetKeyDown(KeyCode.Alpha6))
        {
            Debug.Log("Right Utility");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton6) || Input.GetKeyDown(KeyCode.Tab))
        {
            if (inventory.activeSelf == false)
            {
                inventory.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            else
            {
                inventory.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
            Debug.Log("Inventory");
        }
        if (Input.GetKeyDown(KeyCode.JoystickButton7) || Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Pause");
        }
        if (Input.GetAxisRaw("DPadHori") < 0 || Input.GetKeyDown(KeyCode.Alpha1))
        {
            Debug.Log("Left/1 Inventory");
        }
        if (Input.GetAxisRaw("DPadVert") > 0 || Input.GetKeyDown(KeyCode.Alpha2))
        {
            Debug.Log("Up/2 Inventory");
        }
        if (Input.GetAxisRaw("DPadHori") > 0 || Input.GetKeyDown(KeyCode.Alpha3))
        {
            Debug.Log("Right/3 Inventory");
        }
        if (Input.GetAxisRaw("DPadVert") < 0 || Input.GetKeyDown(KeyCode.Alpha4))
        {
            Debug.Log("Down/4 Inventory");
        }
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Attack");
        }
        if (Input.GetMouseButtonDown(1))
        {
            Debug.Log("RightClick/Left Trigger");
        }
        //camera
        if (Cursor.lockState == CursorLockMode.Locked)
        {
            gameObject.transform.Rotate(Vector3.up, Input.GetAxis("Mouse X") * curRotationRate);//horizontal
            vertConstraints = Mathf.Clamp(Input.GetAxis("Mouse Y") * -curVertRotateRate + vertConstraints, -60f, 60f);
            playerCamera.localRotation = Quaternion.Euler(new Vector3(vertConstraints, 0f, 0f)); //probably vert
        }
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