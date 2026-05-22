using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class speedometer : MonoBehaviour
{
    public float speed;
    private void Start()
    {
        StartCoroutine(CalcSpeed());
    }
    IEnumerator CalcSpeed()
    {
        bool isPlaying = true;
        while (isPlaying)
        {
            Vector3 prevPos = transform.position;
            yield return new WaitForFixedUpdate();
            speed = Mathf.RoundToInt(Vector3.Distance(transform.position, prevPos) / Time.fixedDeltaTime);
        }
    }
}