using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Cube : MonoBehaviour
{
    public int xSize, ySize, zSize;

    void Start()
    {
        Generate();
    }

    private Mesh mesh;
    Vector3[] vertices;
    void Generate()
    {
        GetComponent<MeshFilter>().mesh = mesh = new Mesh();
        mesh.name = "Procedural Cube";

        WaitForSeconds wait = new WaitForSeconds(float.Epsilon);

        CreateVerts();
        CreateTris();
    }

    void CreateVerts()
    {
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
            }

            for (int z = 1; z <= zSize; z++)
            {
                vertices[vert++] = new Vector3(xSize, y, z);
            }

            for (int x = xSize - 1; x >= 0; x--)
            {
                vertices[vert++] = new Vector3(x, y, zSize);
            }

            for (int z = zSize - 1; z > 0; z--)
            {
                vertices[vert++] = new Vector3(0, y, z);
            }
        }

        for (int z = 1; z < zSize; z++)
        {
            for (int x = 1; x < xSize; x++)
            {
                vertices[vert++] = new Vector3(x, 0, z);
                vertices[vert++] = new Vector3(x, ySize, z);
            }
        }

        mesh.vertices = vertices;
    }

    void CreateTris()
    {
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];
        int ring = (xSize + zSize) * 2;
        int t = 0, v = 0;

        for (int y = 0; y < ySize; y++, v++)
        {
            for (int q = 0; q < ring - 1; q++, v++)
            {
                t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
            }
            t = SetQuad(triangles, t, v, v - ring + 1, v + ring, v + 1);
        }
        mesh.triangles = triangles;
        /*
        int quads = (xSize * ySize + xSize * zSize + ySize * zSize) * 2;
        int[] triangles = new int[quads * 6];
        int ring = (xSize + zSize) * 2;
        int t = 0, v = 0;

        for (int q = 0; q < xSize; q++, v++)
        {
            t = SetQuad(triangles, t, v, v + 1, v + ring, v + ring + 1);
        }

        mesh.triangles = triangles;
        */
    }

    public static int SetQuad(int[] triangles, int i, int v00, int v10, int v01, int v11)
    {
        triangles[i] = v00;
        triangles[i + 1] = triangles[i + 4] = v01;
        triangles[i + 2] = triangles[i + 3] = v10;
        triangles[i + 5] = v11;
            
        return i + 6;
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