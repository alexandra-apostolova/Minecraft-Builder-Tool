using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Chunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    List<Vector3> vertices = new();
    List<int> triangles = new();
    List<Vector2> uvs = new();

    int vertexIndex = 0;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        foreach (var block in HouseData.Blocks)
        {
            if (block.Type == "air")
                continue;

            InsertData(new Vector3(block.z, block.y, block.x));
        }

        CreateMesh();
    }

    void InsertData(Vector3 pos)
    {
        for (int i = 0; i < 6; i++)
        {
            for (int j = 0; j < 6; j++)
            {
                int triangleIndex = CubeData.tris[i, j];
                vertices.Add(CubeData.verts[triangleIndex] + pos);
                uvs.Add(CubeData.uvs[j]);

                triangles.Add(vertexIndex);
                vertexIndex++;
            }
        }
    }

    void CreateMesh()
    {
        Mesh mesh = new Mesh();
        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();

        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
    }
}
