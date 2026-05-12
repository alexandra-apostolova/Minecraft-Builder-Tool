using System.Xml.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Block Data", menuName = "Minecraft Builder/Block Data")]
public class BlockData : ScriptableObject
{
    public BlockType[] blockTypes;
}

[System.Serializable]
public class BlockType 
{
    public string blockName;
    public bool isSolid;
    public RenderType renderType;

    //Face Textures:

    public int backFace;
    public int frontFace;
    public int topFace;
    public int bottomFace;
    public int leftFace;
    public int rightFace;
    public int GetTextureId(int faceIndex)
    {
        switch (faceIndex)
        {
            case 0:
                return backFace;
            case 1:
                return frontFace;
            case 2:
                return topFace;
            case 3:
                return bottomFace;
            case 4:
                return leftFace;
            case 5:
                return rightFace;
            default:
                Debug.Log("Error! Invalid faceIndex.");
                return backFace;
        }
    }
}

