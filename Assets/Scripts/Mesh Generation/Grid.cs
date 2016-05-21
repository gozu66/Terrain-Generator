﻿using UnityEngine;
using System.Collections;
//using System.Collections.Generic;

[RequireComponent(typeof(MeshRenderer), typeof(MeshFilter))]
public class Grid : MonoBehaviour
{
    public int xSize, ySize;

    void Start()
    {
        StartCoroutine("Generate"); 
    }

//    private Vector3[] vertices;
//    private Mesh mesh;

    IEnumerator Generate()
    {
        WaitForSeconds wait = new WaitForSeconds(float.Epsilon);

        Mesh mesh = GetComponent<MeshFilter>().mesh = new Mesh();

        Vector3 [] vertices = new Vector3[(xSize + 1) * (ySize + 1)];
        Vector2[] uv = new Vector2[vertices.Length];
        Vector4[] tangents = new Vector4[vertices.Length];
        Vector4 tangent = new Vector4(1f, 0f, 0f, -1f);

        /*
        for (int i = 0, y = 0; y <= ySize; y++)
        {
            for (int x = 0; x <= xSize; x++, i++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
            }
        }
        */

        int i = 0;
        for (int x = 0; x <= xSize; x++)
        {
            for (int y = 0; y <= ySize; y++)
            {
                vertices[i] = new Vector3(x, y);
                uv[i] = new Vector2((float)x / xSize, (float)y / ySize);
                tangents[i] = tangent;
                i++;

                //yield return wait;
            }
        }


        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.tangents = tangents;

        int[] triangles = new int[xSize * ySize * 6];

        /*
                for (int ti = 0, vi = 0, y = 0; y < ySize; y++, vi++)
                {
                    for (int x = 0; x < xSize; x++, ti += 6, vi++)
                    {
                        triangles[ti] = vi;
                        triangles[ti + 3] = triangles[ti + 2] = vi + 1;
                        triangles[ti + 4] = triangles[ti + 1] = vi + xSize + 1;
                        triangles[ti + 5] = vi + xSize + 2;

                        mesh.triangles = triangles;
                        mesh.RecalculateNormals();
                        yield return wait;
                    }
                }
        */
        int tri = 0;
        int vert = 0;
        for(int x = 0; x < xSize; x++)
        {
            for(int y = 0; y < ySize; y++)
            {
                triangles[tri] = vert;
                triangles[tri + 1] = triangles[tri + 4] = vert + 1;
                triangles[tri + 2] = triangles[tri + 3] = vert + ySize + 1;
                triangles[tri + 5] = vert + ySize + 2;

                mesh.triangles = triangles;
                mesh.RecalculateNormals();

                tri += 6;
                vert++;

                yield return wait;
            }
            vert++;
        }
    }
  /*
    void OnDrawGizmos()
    {
        if (vertices == null)
            return;

        Gizmos.color = Color.red;
        for(int i = 0; i < vertices.Length; i++)
        {
            Gizmos.DrawSphere(vertices[i], 0.1f);
        }
    }
   */
}