using System.Collections;
using UnityEngine;

public class LookAt : MonoBehaviour
{
    [SerializeField] public GameObject currentTarget;
    [SerializeField] private float rotateSpeed;
    private Coroutine lookCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        FaceTarget();
    }
    public void FaceTarget()
    {
        if (lookCoroutine == null)
        {
            lookCoroutine = StartCoroutine(LookAts());
        }
        //Vector3 directionToTarget = target.position - transform.position;

        //Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);

        //transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, rotateSpeed * Time.deltaTime);
    }

    public IEnumerator LookAts()
    {
        Quaternion lookRotation = Quaternion.LookRotation(currentTarget.transform.position - transform.position);

        float time = 0;

        while (time < 1)
        {
            transform.localRotation = Quaternion.Slerp(transform.localRotation, lookRotation, time);

            time += Time.deltaTime * rotateSpeed;

            yield return null;
        }
        if (time >= 1)
        {
            lookCoroutine = null;
        }
    }
}
