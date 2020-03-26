using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    private int[] triangles;

    // Start is called before the first frame update
    void Start()
    {
        //Laver et nyt mesh som kan bruges
        mesh = new Mesh();
        //Gør så vi kan tilføje det nye mesh til meshfilteret
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    void Update()
    {
        CreateShape();
        UpdateMesh();
    }
    public void CreateShape()
    {
        vertices = new Vector3[]
        {
            new Vector3(0,0,0),
            new Vector3(0,0,1),
            new Vector3(1,0,0),
            new Vector3(1,0,1)
        };
        triangles = new int[]
        {
            0,1,2,
            1,3,2
        };
    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //Gør så lyset fungerer ordentligt på meshet
        mesh.RecalculateNormals();
    }

}
