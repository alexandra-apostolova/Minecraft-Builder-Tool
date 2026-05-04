using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;
using static Unity.Collections.AllocatorManager;
using static UnityEditor.PlayerSettings;
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
            if (block.Type == "air")
                continue;

            Vector3 pos = new(block.x, block.y, block.z);

            InsertData(pos, block);
        }

        CreateMesh();
    }

    void PopulateVoxelMap()
    {
        foreach (var block in HouseData.Blocks)
        {
            if (block.Type == "air")
                continue;

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
    void InsertData(Vector3 pos, Block block)
    {
        if (CubeData.blockTextureMap.ContainsKey(block.Name))
        {
            CubeData.BlockDefinition blockDef = CubeData.blockTextureMap[block.Name];

            switch (blockDef.RenderType)
            {
                case BlockRenderTypes.Cube:
                    InsertCube(pos, block);
                    break;
                case BlockRenderTypes.Stairs:
                    break;
                case BlockRenderTypes.Slab:
                    break;
                case BlockRenderTypes.Fence:
                    break;
                case BlockRenderTypes.Cross:
                    break;
                case BlockRenderTypes.Custom:
                    break;
                default:
                    break;
            }
        }
        else
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 neighborPos = pos + CubeData.faceChecks[i];
                if (!CheckVoxel(neighborPos))
                {
                    vertices.Add(CubeData.verts[CubeData.tris[i, 0]] + pos);
                    vertices.Add(CubeData.verts[CubeData.tris[i, 1]] + pos);
                    vertices.Add(CubeData.verts[CubeData.tris[i, 2]] + pos);
                    vertices.Add(CubeData.verts[CubeData.tris[i, 3]] + pos);

                    AddTexture(3);

                    triangles.Add(vertexIndex);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 2);
                    triangles.Add(vertexIndex + 1);
                    triangles.Add(vertexIndex + 3);

                    vertexIndex += 4;
                }
            }
        }


    }

    private void InsertCube(Vector3 pos, Block block)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 neighborPos = pos + CubeData.faceChecks[i];
            if (!CheckVoxel(neighborPos))
            {
                vertices.Add(CubeData.verts[CubeData.tris[i, 0]] + pos);
                vertices.Add(CubeData.verts[CubeData.tris[i, 1]] + pos);
                vertices.Add(CubeData.verts[CubeData.tris[i, 2]] + pos);
                vertices.Add(CubeData.verts[CubeData.tris[i, 3]] + pos);

                AddTexture(block.GetTextureId(i));

                triangles.Add(vertexIndex);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 2);
                triangles.Add(vertexIndex + 1);
                triangles.Add(vertexIndex + 3);

                vertexIndex += 4;
            }
        }
    }

    void AddTexture(int textureId)
    {
        float y = textureId / CubeData.textureAtlasSizeInBlocks;
        float x = textureId - (y * CubeData.textureAtlasSizeInBlocks);

        y *= CubeData.normalizedBlockTextureSize;
        x *= CubeData.normalizedBlockTextureSize;

        y = 1f - y - CubeData.normalizedBlockTextureSize;

        uvs.Add(new Vector2(x, y));
        uvs.Add(new Vector2(x, y + CubeData.normalizedBlockTextureSize));
        uvs.Add(new Vector2(x + CubeData.normalizedBlockTextureSize, y));
        uvs.Add(new Vector2(x + CubeData.normalizedBlockTextureSize, y + CubeData.normalizedBlockTextureSize));

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
