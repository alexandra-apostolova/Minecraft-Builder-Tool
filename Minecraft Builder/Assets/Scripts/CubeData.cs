using System.Collections.Generic;
using UnityEngine;

public static class CubeData
{
    public static readonly int textureAtlasSizeInBlocks = 4;
    public static float normalizedBlockTextureSize
    {
        get { return 1f / textureAtlasSizeInBlocks; }
    }

    public static readonly Vector3[] verts = new Vector3[8]
    {
            new Vector3(0.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 0.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 1.0f, 0.0f),
            new Vector3(0.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 0.0f, 1.0f),
            new Vector3(1.0f, 1.0f, 1.0f),
            new Vector3(0.0f, 1.0f, 1.0f),
    };

    public static readonly Vector3[] slabVerts = new Vector3[8]
    {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.5f, 0.0f),
        new Vector3(0.0f, 0.5f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.0f, 1.0f),
        new Vector3(1.0f, 0.5f, 1.0f),
        new Vector3(0.0f, 0.5f, 1.0f),
    };

    public static readonly Vector3[] halfSlabVerts = new Vector3[8]
    {
        new Vector3(0.0f, 0.0f, 0.0f),
        new Vector3(0.5f, 0.0f, 0.0f),
        new Vector3(0.5f, 0.5f, 0.0f),
        new Vector3(0.0f, 0.5f, 0.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.5f, 0.0f, 1.0f),
        new Vector3(0.5f, 0.5f, 1.0f),
        new Vector3(0.0f, 0.5f, 1.0f),
    };

    public static readonly Vector3[] faceChecks = new Vector3[6]
    {
        new Vector3(0.0f, 0.0f, -1.0f),
        new Vector3(0.0f, 0.0f, 1.0f),
        new Vector3(0.0f, 1.0f, 0.0f),
        new Vector3(0.0f, -1.0f, 0.0f),
        new Vector3(-1.0f, 0.0f, 0.0f),
        new Vector3(1.0f, 0.0f, 0.0f),
    };

    public static readonly int[,] tris = new int[6, 4]
    {
       { 0, 3, 1, 2 },
       { 5, 6, 4, 7 },
       { 3, 7, 2, 6 },
       { 1, 5, 0, 4 },
       { 4, 7, 0, 3 },
       { 1, 2, 5, 6 },
    };

    public static readonly Vector2[] uvs = new Vector2[4]
    {
        new Vector2(0.0f, 0.0f),
        new Vector2(0.0f, 1.0f),
        new Vector2(1.0f, 0.0f),
        new Vector2(1.0f, 1.0f),
    };

    public class BlockDefinition
    {
        public BlockRenderTypes RenderType;
        public int[] Textures;
        public bool IsSolid;
    }


    public static readonly Dictionary<string, BlockDefinition> blockTextureMap =
        new Dictionary<string, BlockDefinition>()
    {
        {"oak_planks", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 12 } } },
        {"oak_log", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 14, 15 } } },
        {"oak_stairs", new BlockDefinition{IsSolid = false, RenderType = BlockRenderTypes.Stairs, Textures = new int[] { 12 } } },
        {"oak_slab", new BlockDefinition{IsSolid = false, RenderType = BlockRenderTypes.Slab, Textures = new int[] { 12 } } },
        {"oak_leaves", new BlockDefinition{ IsSolid = false, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 5 } } },
        {"spruce_planks", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 13 } } },
        {"spruce_stairs", new BlockDefinition{IsSolid = false, RenderType = BlockRenderTypes.Stairs, Textures = new int[] { 13 } } },
        {"spruce_slab", new BlockDefinition{IsSolid = false, RenderType = BlockRenderTypes.Slab, Textures = new int[] { 13 } } },
        {"spruce_trapdoor", new BlockDefinition{ IsSolid = false, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 11 } } },
        {"spruce_door", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 6, 7 } } },
        {"grass_block", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 8, 9, 10 } } },
        {"dirt", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 10 } } },
        {"stone", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 1 } } },
        {"cobblestone", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 0 } } },
        {"decorated_pot", new BlockDefinition{ IsSolid = true, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 4 } } },
        {"glass_pane", new BlockDefinition{ IsSolid = false, RenderType = BlockRenderTypes.Cube, Textures = new int[]{ 2 } } },
    };

}
