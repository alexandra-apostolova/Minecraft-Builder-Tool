
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GenerateChunk : MonoBehaviour
{
    public MeshRenderer meshRenderer;
    public MeshFilter meshFilter;
    public BlockData blockData;
    public RenderType[] renderTypes;

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
        BlockType blockInfo = blockData.blockTypes.FirstOrDefault(n => n.blockName == block.Name);
        if (blockInfo != null)
        {
            for (int i = 0; i < 6; i++)
            {
                Vector3 neighborPos = pos + CubeData.faceChecks[i];
                AddFace(blockInfo, pos, new Vector3(0.0f, 0.0f, 0.0f), i, block);
                //if (!CheckVoxel(neighborPos))
                //{
                //}
            }
        }
    }

    private void AddFace(BlockType blockInfo, Vector3 pos,
        Vector3 offsets, int faceIndex, Block block)
    {
        int faceVertCount = 0;
        Vector3 center = new Vector3(0.5f, 0.5f, 0.5f);
        Quaternion rotation = GetHorizontalRotation(block.Facing);

        for (int j = 0; j < blockInfo.renderType.faces[faceIndex].vertData.Length; j++)
        {
            Vector3 vert = blockInfo.renderType.faces[faceIndex].vertData[j].position;

            vert += offsets;
            vert = rotation * (vert - center) + center;
            vertices.Add(vert + pos);

            AddTexture(blockInfo.GetTextureId(faceIndex), blockInfo.renderType.faces[faceIndex].vertData[j].uv);
            faceVertCount++;
        }

        for (int i = 0; i < blockInfo.renderType.faces[faceIndex].triangles.Length; i++)
        {
            triangles.Add(vertexIndex + blockInfo.renderType.faces[faceIndex].triangles[i]);
        }

        vertexIndex += faceVertCount;
    }
    private Quaternion GetHorizontalRotation(string direction)
    {
        switch (direction)
        {
            case "north":
                return Quaternion.Euler(0, 270, 0);
            case "east":
                return Quaternion.Euler(0, 180, 0);
            case "south":
                return Quaternion.Euler(0, 90, 0);
            case "west":
                return Quaternion.Euler(0, 0, 0);
            default:
                return Quaternion.Euler(0, 0, 0);
        }
    }

    //private void InsertStairs(Vector3 pos, Block block)
    //{
    //    InsertSlab(pos, block);
    //    InsertHalfSlab(pos, block);
    //}

    //private void InsertHalfSlab(Vector3 pos, Block block)
    //{
    //    float yOffset = (block.Half == "top") ? 0.0f : 0.5f;
    //    for (int i = 0; i < 6; i++)
    //    {
    //        AddFace(CubeData.halfSlabVerts, pos, new Vector3(0.0f, yOffset, 0.0f), i, block);
    //    }
    //}

    //private void InsertSlab(Vector3 pos, Block block)
    //{
    //    float yOffset = (block.Half == "top") ? 0.5f : 0.0f;
    //    for (int i = 0; i < 6; i++)
    //    {
    //        AddFace(CubeData.slabVerts, pos, new Vector3(0.0f, yOffset, 0.0f), i, block);
    //    }
    //}

    //private void InsertCube(Vector3 pos, Block block)
    //{
    //    for (int i = 0; i < 6; i++)
    //    {
    //        Vector3 neighborPos = pos + CubeData.faceChecks[i];
    //        AddFace(CubeData.verts, pos, new Vector3(0.0f, 0.0f, 0.0f), i, block);
    //        //if (!CheckVoxel(neighborPos))
    //        //{
    //        //}
    //    }
    //}

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

        Debug.Log(mesh.vertexCount);
        Debug.Log(mesh.triangles.Length);

        meshFilter.mesh = mesh;
    }

}
