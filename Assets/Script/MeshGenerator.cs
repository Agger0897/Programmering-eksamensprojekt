using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshGenerator : MonoBehaviour
{
    Mesh mesh;                                      //Meshet der bruges til terrænet

    Vector3[] vertices;                             //Liste over alle punkterne 
    private int[] triangles;                        //Liste over alle trekanterne 

    public float amplitude = 2f;                    //Gør at der kan manipuleres med y-aksen i inspektoren 
    
    public int XSize;                               //Bestemmer størrelsen på X-aksen af griddet 
    public int ZSize;                               //Bestemmer størrelsen på Z-aksen af griddet

    public bool activateGizmos;                     //Bool der gør at punkterne kan visualiseres i scenen

    // Start is called before the first frame update
    private void Start()
    {
        mesh = new Mesh();                          //Laver et nyt mesh som kan bruges
        GetComponent<MeshFilter>().mesh = mesh;     //Gør så vi kan tilføje det nye mesh til meshfilteret
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
                float y = Mathf.PerlinNoise(x * .5f, z * .3f) * amplitude;      //Her udregnes y-koordinaten for alle punkterne
                vertices[i] = new Vector3(x, y, z);                             //Her genereres dataen til hver punkt
                i++;                                                            //Går til det næste element i listen
            }
        }

        //Liste over alle trekanterne
        //gange 6 da 2 trekanter kræver 6 punkter
        triangles = new int[XSize * ZSize * 6];
        
        int vertexIndex = 0;                //Nulstiller listen over alle punkterne
        int triangleIndex = 0;              //Nulstiller listen over alle trekanterne

        //Går z-aksen igennem
        for (int z = 0; z < ZSize; z++)
        {
            //Går x-aksen igennem
            for (int x = 0; x < XSize; x++)
            {
                //En trekant
                triangles[triangleIndex + 0] = vertexIndex + 0;
                triangles[triangleIndex + 1] = vertexIndex + XSize + 1;
                triangles[triangleIndex + 2] = vertexIndex + 1;

                //En anden trekant som sammen med den ande trekant danner en firkant
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
        mesh.Clear();                                                           //Fjerner tidligere data fra meshet        
        mesh.vertices = vertices;                                               //Placerer alle punkterne i scenen
        mesh.triangles = triangles;                                             //Placerer alle trekanterne i scenen

        mesh.RecalculateNormals();                                              //Gør så lyset fungerer ordentligt på meshet
        mesh.RecalculateBounds();                                               //Gør så terrænet collider ordentligt med spilleren
        MeshCollider meshCollider = gameObject.GetComponent<MeshCollider>();    //Henter meshets collider
        meshCollider.sharedMesh = mesh;                                         //Gør så terrænet collider med spilleren
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
