using System.Collections.Generic;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;

public class PlaneModifier : MonoBehaviour
{

    Mesh myMesh;
    MeshFilter meshFilter;

    [SerializeField] Vector2 planeSize = new Vector2(1, 1);
    [SerializeField] int planeResolution = 1;
    [SerializeField] float heightSeverity = 1;



    List<Vector3> vertices;
    List<int> triangles;



    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        myMesh = new Mesh();
        meshFilter = GetComponent<MeshFilter>();
        meshFilter.mesh = myMesh;
        planeResolution = Mathf.Clamp(planeResolution, 1, 1000);

        GeneratePlane(planeSize, planeResolution);
        initialRandomPlaneTest(planeResolution, heightSeverity);
        AssignMesh();
    }

    // Update is called once per frame
    void Update()
    {
        /*
        planeResolution = Mathf.Clamp(planeResolution, 1, 1000);

        GeneratePlane(planeSize, planeResolution);
        initialRandomPlaneTest();
        AssignMesh();
        */
    }

    void GeneratePlane(Vector2 size, int resolution)
    {

        vertices = new List<Vector3>();
        float xPerStep = size.x / resolution;
        float yPerStep = size.y / resolution;
        for (int y = 0; y < resolution + 1; y++)
        {
            for (int x = 0; x < resolution + 1; x++)
            {
                vertices.Add(new Vector3(leftEdge(y * yPerStep) + x * xPerStep, 0, y * yPerStep));
            }
        }


        triangles = new List<int>();
        for (int row = 0; row < resolution; row++)
        {
            for (int column = 0; column < resolution; column++)
            {
                int i = (row * resolution) + row + column;

                //triangle 1
                triangles.Add(i);
                triangles.Add(i + resolution + 1);
                triangles.Add(i + resolution + 2);

                //triangle 2
                triangles.Add(i);
                triangles.Add(i + resolution + 2);
                triangles.Add(i + 1);
            }
        }

    }

    void AssignMesh()
    {
        myMesh.Clear();
        myMesh.vertices = vertices.ToArray();
        myMesh.triangles = triangles.ToArray();
        DestroyImmediate(this.GetComponent<MeshCollider>());
        var collider = this.AddComponent<MeshCollider>();
        collider.sharedMesh = myMesh;
    }

    void initialRandomPlaneTest(int resolution, float height)
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            if (i < resolution || i > resolution * (resolution + 1) || i % (resolution + 1) == 0 || i % (resolution + 1) == resolution)
            {
                vertex.y = 0;
            }
            else vertex.y = Random.Range(-height, height);
            vertices[i] = vertex;
        }
    }

    float leftEdge(float yValue)
    {
        float xValue = yValue / (Mathf.Sqrt(3));
        return xValue;
    }
    float rightEdge(float yValue, Vector2 size)
    {
        float xValue = yValue * (-Mathf.Sqrt(3) / 2) + size.x;
        return xValue;
    }


}
