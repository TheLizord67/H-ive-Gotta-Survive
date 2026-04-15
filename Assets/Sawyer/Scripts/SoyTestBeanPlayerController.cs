using UnityEngine;
using UnityEngine.InputSystem;

public class SoyTestBeanPlayerController : MonoBehaviour
{
    private Rigidbody rb;
    [SerializeField] float speed;
    private float cammyPitch;
    [SerializeField] float camSensitivity;
    [SerializeField] GameObject cameraHH;
    [SerializeField] float jumpVelocity;
    private float lastJumpTime;
    [SerializeField] float jumpCooldown;
    [SerializeField] GameObject foundationPrefab;
    private bool isPreviewing = false;
    private GameObject previewStructure;
    [SerializeField] Material previewMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
       
        //rb.linearVelocity = transform.rotation * new Vector3(Input.GetAxis("Horizontal") * speed, rb.linearVelocity.y, Input.GetAxis("Vertical") * speed);
        rb.linearVelocity = transform.rotation * new Vector3(((Keyboard.current.rightArrowKey.IsPressed() ? 1 : 0) - (Keyboard.current.leftArrowKey.IsPressed() ? 1 : 0)) * speed, rb.linearVelocity.y, ((Keyboard.current.upArrowKey.IsPressed() ? 1 : 0) - (Keyboard.current.downArrowKey.IsPressed() ? 1 : 0)) * speed);
        transform.Rotate(0, ((Keyboard.current.dKey.IsPressed() ? 1 : 0) - (Keyboard.current.aKey.IsPressed() ? 1 : 0)) * camSensitivity, 0);
        cammyPitch = Mathf.Clamp(cammyPitch - ((Keyboard.current.wKey.IsPressed() ? 1 : 0) - (Keyboard.current.sKey.IsPressed() ? 1 : 0)) * camSensitivity, -90, 90);
        cameraHH.transform.localRotation = Quaternion.Euler(cammyPitch, 0, 0);

        if (Keyboard.current.spaceKey.IsPressed() && (Time.time > lastJumpTime + jumpCooldown))
        {
            rb.linearVelocity = new Vector3(rb.linearVelocity.x, rb.linearVelocity.y + jumpVelocity, rb.linearVelocity.z);
           lastJumpTime = Time.time;
        }

        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isPreviewing)
            {
                Debug.DrawLine(transform.position, previewStructure.transform.position, Color.violetRed, 5f);
                Instantiate(foundationPrefab, previewStructure.transform.position, transform.rotation);
                Destroy(previewStructure);
                isPreviewing = false;
            }
            else
            {
                previewStructure = Instantiate(foundationPrefab, transform.position, transform.rotation);
                previewStructure.GetComponent<MeshRenderer>().material = previewMaterial;
                previewStructure.GetComponent<Rigidbody>().detectCollisions = false;
                isPreviewing = true;
            }
            
        }

        if (isPreviewing)
        {
            RaycastHit hit;
            if (Physics.Raycast(transform.position, cameraHH.transform.TransformDirection(Vector3.forward), out hit, 15))
            {
                previewStructure.transform.position = hit.point;
                previewStructure.transform.rotation = transform.rotation;
                Debug.DrawLine(transform.position, previewStructure.transform.position, Color.violetRed, 0.1f);
            }
        }


    }
}
