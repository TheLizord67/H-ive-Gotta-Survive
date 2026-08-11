using UnityEngine;

public class jumpBecauseFuckYou : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Joystick1Button0))
        {
            this.gameObject.GetComponent<Rigidbody>().angularVelocity = new Vector3(0, 0, 0); // Reset angular velocity
            this.gameObject.GetComponent<Rigidbody>().AddForce(Vector3.up * this.gameObject.GetComponent<playerController>().jumpForce, ForceMode.VelocityChange);
        }
    }
}
