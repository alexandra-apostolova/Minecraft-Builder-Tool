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

    public static readonly Dictionary<string, int[]> blockTextureMap = new Dictionary<string, int[]>()
    {
        {"oak_planks", new int[]{ 12 } },
        {"oak_log", new int[]{ 14, 15 } },
        {"spruce_planks", new int[]{ 13 } },
        {"spruce_trapdoor", new int[]{ 11 } },
        {"spruce_door", new int[]{ 6, 7 } },
        {"grass_block", new int[]{ 8, 9, 10 } },
        {"dirt", new int[]{ 10 } },
        {"stone", new int[]{ 1 } },
        {"cobblestone", new int[]{ 0 } },
        {"decorated_pot", new int[]{ 4 } },
        {"glass_pane", new int[]{ 2 } }
    };

}
