using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class GenerateChunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;

    List<Vector3> vertices = new();
    List<int> triangles = new();
    List<Vector2> uvs = new();

    int vertexIndex = 0;
    bool[,,] voxelMap;
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshFilter = GetComponent<MeshFilter>();

        voxelMap = new bool[HouseData.chunkWidth, HouseData.chunkHeight, HouseData.chunkLength];

        PopulateVoxelMap();

        foreach (var block in HouseData.Blocks)
        {
            Vector3 pos = new(block.x, block.y, block.z);

            InsertData(pos);
        }

        CreateMesh();
    }

    void PopulateVoxelMap()
    {
        foreach (var block in HouseData.Blocks)
        {
            voxelMap[block.x, block.y, block.z] = true;
        }
    }

    bool CheckVoxel(Vector3 pos)
    {
        Vector3Int posInt = Vector3Int.FloorToInt(pos);

        if (posInt.x < 0 || posInt.x > HouseData.chunkWidth - 1 ||
            posInt.y < 0 || posInt.y > HouseData.chunkHeight - 1 ||
            posInt.z < 0 || posInt.z > HouseData.chunkLength - 1)
        {
            return false;
        }

        return voxelMap[posInt.x, posInt.y, posInt.z];
    }
    void InsertData(Vector3 pos)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 neighborPos = pos + CubeData.faceChecks[i];
            if (!CheckVoxel(neighborPos))
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
