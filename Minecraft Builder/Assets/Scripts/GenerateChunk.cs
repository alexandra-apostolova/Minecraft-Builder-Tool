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
                    InsertStairs(pos, block);
                    break;
                case BlockRenderTypes.Slab:
                    InsertSlab(pos, block);
                    break;
                case BlockRenderTypes.Fence:
                    break;
                case BlockRenderTypes.Cross:
                    break;
                case BlockRenderTypes.Custom:
                    break;
                default:
                    InsertCube(pos, block);
                    break;
            }
        }
    }

    private void AddFace(Vector3[] verts, Vector3 pos, 
        Vector3 offsets, int faceIndex, Block block)
    {
        Vector3 center = new Vector3(0.5f, 0.5f, 0.5f);
        Quaternion rotation = GetRotation(block.Facing);
        for (int j = 0; j < 4; j++)
        {
            Vector3 vert = verts[CubeData.tris[faceIndex, j]];
            vert += offsets;

            vert = rotation * (vert - center) + center;
            vertices.Add(vert + pos);

            if (CubeData.blockTextureMap.ContainsKey(block.Name))
            {
                switch (CubeData.blockTextureMap[block.Name].RenderType)
                {
                    case BlockRenderTypes.Slab:
                    case BlockRenderTypes.Stairs:
                        AddTexture(block.GetTextureId(faceIndex), CubeData.uvsSlabs[j]);
                        break;
                    default:
                        AddTexture(block.GetTextureId(faceIndex), CubeData.uvs[j]);
                        break;
                }
                
            }
            else
            {
                AddTexture(3, CubeData.uvs[j]);
            }
        }

        triangles.Add(vertexIndex);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 2);
        triangles.Add(vertexIndex + 1);
        triangles.Add(vertexIndex + 3);

        vertexIndex += 4;
    }

    private void InsertStairs(Vector3 pos, Block block)
    {
        InsertSlab(pos, block);
        InsertHalfSlab(pos, block);
    }

    private Quaternion GetRotation(string direction)
    {
        switch (direction)
        {
            case "north":
                return Quaternion.Euler(0, 270, 0);
            case "east":
                return Quaternion.Euler(0, 0, 0);
            case "south":
                return Quaternion.Euler(0, 90, 0);
            case "west":
                return Quaternion.Euler(0, 180, 0);
            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }

    private void InsertHalfSlab(Vector3 pos, Block block)
    {
        Vector3 offset = new Vector3(0.0f, 0.5f, 0.0f);
        for (int i = 0; i < 6; i++)
        {
            AddFace(CubeData.halfSlabVerts, pos, offset, i, block);
        }
    }

    private void InsertSlab(Vector3 pos, Block block)
    {
        float yOffset = (block.Half == "top") ? 0.5f : 0.0f;
        for (int i = 0; i < 6; i++)
        {
            AddFace(CubeData.slabVerts, pos, new Vector3(0.0f, yOffset, 0.0f), i, block);
        }
    }

    private void InsertCube(Vector3 pos, Block block)
    {
        for (int i = 0; i < 6; i++)
        {
            Vector3 neighborPos = pos + CubeData.faceChecks[i];
            if (!CheckVoxel(neighborPos))
            {
                AddFace(CubeData.verts, pos, new Vector3(0.0f, 0.0f, 0.0f), i, block);
            }
        }
    }

    void AddTexture(int textureId, Vector2 uv)
    {
        float y = textureId / CubeData.textureAtlasSizeInBlocks;
        float x = textureId - (y * CubeData.textureAtlasSizeInBlocks);

        y *= CubeData.normalizedBlockTextureSize;
        x *= CubeData.normalizedBlockTextureSize;

        y = 1f - y - CubeData.normalizedBlockTextureSize;

        x += CubeData.normalizedBlockTextureSize * uv.x;
        y += CubeData.normalizedBlockTextureSize * uv.y;

        uvs.Add(new Vector2(x, y));
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
