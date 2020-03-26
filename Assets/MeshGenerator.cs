using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;

    Vector3[] vertices;
    private int[] triangles;
    
    //Bestemmer størrelsen på grid
    public int xSize;
    public int ZSize;

    // Start is called before the first frame update
    void Start()
    {
        //Laver et nyt mesh som kan bruges
        mesh = new Mesh();
        //Gør så vi kan tilføje det nye mesh til meshfilteret
        GetComponent<MeshFilter>().mesh = mesh;
        StartCoroutine(CreateShape());
    }

    // Update is called once per frame
    void Update()
    {
        UpdateMesh(); //Skal være i Void Start() hvis det skal gå hurtigt
        OnDrawGizmos();
    }
    IEnumerator CreateShape() //Void hvis det skal gå hurtigere
    {
        //Skaber alle hjørnerne 
        //og da der altid vil være et hjørne mere end længden på griddet lægges der 1 til
        vertices = new Vector3[(xSize + 1) * (ZSize + 1)];

        //Index over alle vertices i griddet
        int i = 0;

        //Går alle vertices igennem og giver dem en position på griddet
        for (int z = 0; z <= ZSize; z++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                float y = Mathf.PerlinNoise(x*.3f, z*.3f) * 2f;
                vertices[i] = new Vector3(x, y, z);
                i++;
            }
        }

        triangles = new int[xSize * ZSize * 6];

        int vert = 0;
        int tris = 0;

        for (int z = 0; z < ZSize; z++)
        {
            for (int x = 0; x < xSize; x++)
            {
                triangles[tris + 0] = vert + 0;
                triangles[tris + 1] = vert + xSize + 1;
                triangles[tris + 2] = vert + 1;
                triangles[tris + 3] = vert + 1;
                triangles[tris + 4] = vert + xSize + 1;
                triangles[tris + 5] = vert + xSize + 2;

                vert++;
                tris += 6;

                yield return new WaitForSeconds(.05f); //Fjern hvis det skal gå hurtigt
            }
            vert++;
        }

    }
    public void UpdateMesh()
    {
        mesh.Clear();
        mesh.vertices = vertices;
        mesh.triangles = triangles;

        //Gør så lyset fungerer ordentligt på meshet
        mesh.RecalculateNormals();
    }

    private void OnDrawGizmos()
    {
        //Hvis der ikke er nogen vertices, skal der ikke gøres noget
        if (vertices == null)
        {
            return;
        }

        //Løb alle vertices igennem og lav en cirkel omkring dem
        for (int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], .1f);
        }
    }

}
