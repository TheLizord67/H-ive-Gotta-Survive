using System;
using UnityEngine;

public class StructureScript : MonoBehaviour
{
    private bool isPreview = false;
    [Serializable]
    public struct SnapPoint
    {
        public Vector3 localPosition;
        public int snapTypeTag;
        public int[] snapFromTypeTags;
        public bool occupied;
    }
    public SnapPoint[] snapPoints;
    // Start is called once before the first execution of Update after the MonoBehaviour is created



    void Start()
    {
        if (GetComponent<Rigidbody>().detectCollisions == false)
        {
            isPreview = true;
        }
        
        if (!isPreview)
        {
            foreach (var SnapPoint in snapPoints)
            {
                
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
