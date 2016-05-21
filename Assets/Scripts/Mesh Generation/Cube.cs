using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Cube : MonoBehaviour
{
    public int xSize, ySize, zSize;

    void Start()
    {
        StartCoroutine("Generate");
    }

    private Mesh mesh;
    Vector3[] vertices;

    IEnumerator Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        WaitForSeconds wait = new WaitForSeconds(float.Epsilon);
/*
        int cornerVerts = 8;
        int edgeVerts = (xSize + ySize + zSize - 3) * 4;
        int faceVerts = (
            (xSize - 1) * (ySize - 1) +
            (xSize - 1) * (zSize - 1) +
            (ySize - 1) * (zSize - 1)) * 2;
*/

        int cornerVerts = 8;
        int edgeVerts = (xSize + ySize + zSize - 3) * 4;
        int faceVerts = (
            (xSize - 1) * (ySize - 1) + 
            (xSize - 1) * (zSize - 1) + 
            (ySize - 1) * (zSize - 1)) * 2;

        vertices = new Vector3[cornerVerts + edgeVerts + faceVerts];

        int vert = 0;
        for (int y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++)
            {
                vertices[vert++] = new Vector3(x, y, 0);
                print("hit");
                yield return wait;
            }

            for (int z = 1; z < zSize; z++)
            {
                vertices[vert++] = new Vector3(xSize, y, z);
                yield return wait;
            }

            for (int x = xSize; x >= 0; x--)
            {
                vertices[vert++] = new Vector3(x, y, zSize);
                yield return wait;
            }

            for (int z = zSize; z >= 0; z--)
            {
                vertices[vert++] = new Vector3(0, y, z);
                yield return wait;
            }
            yield return wait;
        }
    }

    void OnDrawGizmos()
    {
        if(vertices == null)
        {
            return;
        }

        Gizmos.color = Color.black;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
}