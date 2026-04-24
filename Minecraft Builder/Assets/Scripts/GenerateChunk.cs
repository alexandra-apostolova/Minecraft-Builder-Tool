using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        List<Vector3> vertices = new();
        List<int> triangles = new();
        List<Vector2> uvs = new();

        int vertexIndex = 0;

        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int triangleIndex = CubeData.tris[i,j];
                vertices.Add(CubeData.verts[triangleIndex]);
                uvs.Add(CubeData.uvs[j]);

                triangles.Add(vertexIndex);
                Debug.Log(triangles[vertexIndex]);
                vertexIndex++;
            }
        }

        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }

}
