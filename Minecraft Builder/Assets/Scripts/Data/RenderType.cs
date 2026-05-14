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

    public Vector3 GetRotatedPosition(BlockFacing facing)
    {
        Vector3 angles;
        switch (facing)
        {
            case BlockFacing.North:
                angles = new Vector3(0, 0, 0);
                break;
            case BlockFacing.East:
                angles = new Vector3(0, 270, 0);
                break;
            case BlockFacing.South:
                angles = new Vector3(0, 180, 0);
                break;
            case BlockFacing.West:
                angles = new Vector3(0, 90, 0);
                break;
            default:
                angles = new Vector3(0, 0, 0);
                break;
        }

        Vector3 center = new Vector3(0.5f, 0.5f, 0.5f);
        Vector3 direction = position - center;
        direction = Quaternion.Euler(angles) * direction;
        return direction + center;
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