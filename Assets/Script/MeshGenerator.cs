﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;
    MeshCollider meshCollider = null;

    Vector3[] vertices;
    private int[] triangles;

    public float amplitude = 2f;

    //Bestemmer størrelsen på grid
    public int XSize;
    public int ZSize;

    private float minTerrainHeight;
    private float maxTerrainHeight;

    public bool activateGizmos;

    // Start is called before the first frame update
    private void Start()
    {
        //Laver et nyt mesh som kan bruges
        mesh = new Mesh();
        //Gør så vi kan tilføje det nye mesh til meshfilteret
        GetComponent<MeshFilter>().mesh = mesh;
    }

    // Update is called once per frame
    private void Update()
    {
        CreateShape();
        UpdateMesh();
        mesh.Optimize();
    }
    private void CreateShape() //Void hvis det skal gå hurtigere
    {
        //Skaber alle hjørnerne 
        //og da der altid vil være et hjørne mere end længden på griddet lægges der 1 til
        vertices = new Vector3[(XSize + 1) * (ZSize + 1)];

        //Index over alle vertices i griddet
        //Går alle vertices igennem og giver dem en position på griddet
        for (int i = 0, z = 0; z <= ZSize; z++)
        {
            for (int x = 0; x <= XSize; x++)
            {
                float y = Mathf.PerlinNoise(x * .5f, z * .3f) * amplitude;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[XSize * ZSize * 6];

        int vertexIndex = 0;
        int triangleIndex = 0;

        for (int z = 0; z < ZSize; z++)
        {
            for (int x = 0; x < XSize; x++)
            {
                triangles[triangleIndex + 0] = vertexIndex + 0;
                triangles[triangleIndex + 1] = vertexIndex + XSize + 1;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + XSize + 1;
                triangles[triangleIndex + 5] = vertexIndex + XSize + 2;

                vertexIndex++;
                triangleIndex += 6;
            }
            vertexIndex++;
        }
    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //Gør så lyset fungerer ordentligt på meshet
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();
        meshCollider.sharedMesh = mesh;
    }

    private void OnDrawGizmos()
    {
        //Hvis der ikke er nogen vertices, skal der ikke gøres noget
        if (vertices == null)
        {
            return;
        }
        if (activateGizmos == true)
        {
            //Løb alle vertices igennem og lav en cirkel på punkterne
            for (int i = 0; i < vertices.Length; i++)
            {
                Gizmos.DrawSphere(vertices[i], .1f);
            }
        }

    }

}
