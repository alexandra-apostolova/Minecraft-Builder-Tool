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
            new Vector3(0.0f, 0.0f, 0.0f), //0
            new Vector3(1.0f, 0.0f, 0.0f), //1
            new Vector3(1.0f, 1.0f, 0.0f), //2
            new Vector3(0.0f, 1.0f, 0.0f), //3
            new Vector3(0.0f, 0.0f, 1.0f), //4
            new Vector3(1.0f, 0.0f, 1.0f), //5
            new Vector3(1.0f, 1.0f, 1.0f), //6
            new Vector3(0.0f, 1.0f, 1.0f), //7
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


}
