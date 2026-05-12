using UnityEngine;

[CreateAssetMenu(fileName = "New Render Type", menuName = "Minecraft Builder/Render Type")]
public class RenderType : ScriptableObject
{
    public string renderType;
    public FaceMeshData[] faces;
}

[System.Serializable]
public class VertData
{
    public Vector3 position;
    public Vector2 uv;

    public VertData(Vector3 pos, Vector2 _uv)
    {
        position = pos;
        uv = _uv;
    }
}

[System.Serializable]
public class FaceMeshData
{
    public string direction;
    public Vector3 normal;
    public VertData[] vertData;
    public int[] triangles;
}