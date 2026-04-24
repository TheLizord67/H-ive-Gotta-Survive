using System.Collections.Generic;
using System.Xml.Schema;
using UnityEngine;

public class PlaneModifier : MonoBehaviour
{

    Mesh myMesh;
    MeshFilter meshFilter;

    [SerializeField] Vector2 planeSize = new Vector2(1, 1);
    [SerializeField] int planeResolution = 1;

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
        initialRandomPlaneTest();
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
                if (x == 0 || x == resolution)
                {
                    vertices.Add(new Vector3(leftEdge(y) + x * xPerStep, 0, y * yPerStep));
                }
                else if (y == 0 || y == resolution)
                {
                    vertices.Add(new Vector3(leftEdge(y) + x * xPerStep + Random.Range(-xPerStep / 2, xPerStep / 2), 0, y * yPerStep));
                }
                else
                {
                    vertices.Add(new Vector3(leftEdge(y) + x * xPerStep + Random.Range(-xPerStep / 2, xPerStep / 2), 0, y * yPerStep + Random.Range(-yPerStep / 2, yPerStep / 2)));
                }
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
    }

    void initialRandomPlaneTest()
    {
        for (int i = 0; i < vertices.Count; i++)
        {
            Vector3 vertex = vertices[i];
            vertex.y = Random.Range(-10f, 10f);
            vertices[i] = vertex;
        }
    }

    float leftEdge(float yValue)
    {
        float xValue = yValue * (-Mathf.Sqrt(3) / 2);
        return xValue;
    }
    float rightEdge(float yValue, Vector2 size)
    {
        float xValue = yValue * (-Mathf.Sqrt(3) / 2) + size.x;
        return xValue;
    }


}
