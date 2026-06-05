using UnityEngine;
using UnityEngine.InputSystem;

public class SoyTestBeanPlayerController : MonoBehaviour
{
    [SerializeField] private Camera cam;
    [SerializeField] GameObject foundationPrefab;
    private bool isPreviewing = false;
    private GameObject previewStructure;
    [SerializeField] Material previewMaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //cam = GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            Destroy(previewStructure);
            isPreviewing = false;
        }

            if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (isPreviewing)
            {
                Debug.DrawLine(transform.position, previewStructure.transform.position, Color.violetRed, 5f);
                Instantiate(foundationPrefab, previewStructure.transform.position, previewStructure.transform.rotation);
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
            if (Physics.Raycast(transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 15, LayerMask.GetMask("Structure")))
            {
                StructureScript.SnapPoint[] snapsToCheck = hit.collider.gameObject.GetComponent<StructureScript>().snapPoints;

                int closestPoint = 0;
                float closestPointVal = 1000;
                for (int i = 0; i < snapsToCheck.Length; i++)
                {
                    if (!snapsToCheck[i].occupied)
                    {
                        if (closestPointVal > Vector3.Distance(hit.point, hit.collider.gameObject.transform.rotation * snapsToCheck[i].localPosition + hit.collider.gameObject.transform.position))
                        {
                            closestPoint = i;
                            closestPointVal = Vector3.Distance(hit.point, hit.collider.gameObject.transform.rotation * snapsToCheck[i].localPosition + hit.collider.gameObject.transform.position);
                        }

                    }
                }
                if (closestPointVal < 1000)
                {
                    previewStructure.transform.position = hit.collider.gameObject.transform.rotation * snapsToCheck[closestPoint].localPosition + hit.collider.gameObject.transform.position;
                    previewStructure.transform.rotation = hit.collider.gameObject.transform.rotation;
                    Debug.DrawLine(transform.position, previewStructure.transform.position, Color.violetRed, 0.1f);
                }
                else
                {
                    previewStructure.transform.position = hit.point;
                    previewStructure.transform.rotation = transform.rotation;
                    Debug.DrawLine(transform.position, previewStructure.transform.position, Color.violetRed, 0.1f);
                }

                    
            }

            else if (Physics.Raycast(transform.position, cam.transform.TransformDirection(Vector3.forward), out hit, 15))
            {
                previewStructure.transform.position = hit.point;
                previewStructure.transform.rotation = transform.rotation;
                Debug.DrawLine(transform.position, previewStructure.transform.position, Color.violetRed, 0.1f);
            }
        }


    }
}
