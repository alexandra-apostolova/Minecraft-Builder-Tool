using fNbt;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
public class ReadSchema : MonoBehaviour
{
    void Awake()
    {
        NbtFile file = new NbtFile();

        string path = Path.Combine(Application.streamingAssetsPath, "houseWithDecorations.schem");
        file.LoadFromFile(path);

        NbtCompound root = file.RootTag;
        NbtCompound schematic = root["Schematic"] as NbtCompound;

        HouseData.chunkWidth = schematic.Get<NbtShort>("Width").Value;
        HouseData.chunkHeight = schematic.Get<NbtShort>("Height").Value;
        HouseData.chunkLength = schematic.Get<NbtShort>("Length").Value;

        NbtCompound blocks = schematic.Get<NbtCompound>("Blocks");
        NbtCompound paletteTag = blocks.Get<NbtCompound>("Palette");

        Dictionary<int, string> palette = new Dictionary<int, string>();
        foreach (var tag in paletteTag.Tags)
        {
            int id = tag.IntValue;
            string blockName = tag.Name.Substring(10);

            palette[id] = blockName;
        }

        NbtByteArray blockDataTag = blocks.Get<NbtByteArray>("Data");
        byte[] blockData = blockDataTag.Value;

        List<Block> blocksList = new();

        for (int i = 0; i < blockData.Length; i++)
        {
            int paletteIndex = blockData[i];
            if (!palette.ContainsKey(paletteIndex))
            {
                continue;
            }

            Vector3Int blockPos = IndexToPos(i, HouseData.chunkWidth, HouseData.chunkLength);

            Block blockToAdd = new Block
            {
                x = blockPos.x,
                y = blockPos.y,
                z = blockPos.z,
                Type = palette[paletteIndex]
            };

            ReadOnlySpan<char> fullId = blockToAdd.Type;

            if (blockToAdd.Type.Contains('['))
            {
                int openBracketIndex = fullId.IndexOf('[');
                int closeBracketIndex = fullId.IndexOf(']');

                ReadOnlySpan<char> name = fullId.Slice(0, openBracketIndex);
                blockToAdd.Name = name.ToString();

                ReadOnlySpan<char> inner = fullId.Slice(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);

                while (!inner.IsEmpty)
                {
                    int comaIndex = inner.IndexOf(',');

                    ReadOnlySpan<char> pair;
                    if (comaIndex == -1)
                    {
                        pair = inner;
                        inner = ReadOnlySpan<char>.Empty;
                    }
                    else
                    {
                        pair = inner.Slice(0, comaIndex);
                        inner = inner.Slice(comaIndex + 1);
                    }

                    int equalsIndex = pair.IndexOf("=");
                    ReadOnlySpan<char> key = pair.Slice(0, equalsIndex);
                    ReadOnlySpan<char> value = pair.Slice(equalsIndex + 1);

                    if (key.ToString() == "facing")
                    {
                        blockToAdd.Facing = value.ToString();
                    }
                    else if (key.ToString() == "half" || key.ToString() == "type")
                    {
                        blockToAdd.Half = value.ToString();
                    }
                    else if (key.ToString() == "axis")
                    {
                        blockToAdd.Axis = value.ToString();
                    }
                }
            }
            else
            {
                blockToAdd.Name = blockToAdd.Type;
            }

            blocksList.Add(blockToAdd);
        }

        HouseData.Blocks = blocksList;
    }
    static Vector3Int IndexToPos(int index, int width, int length)
    {
        int x = index % width;
        int y = index / (width * length);
        int z = (index / width) % length;

        return new Vector3Int(x, y, z);
    }
}

public class Block
{
    public int x;
    public int z;
    public int y;
    public string Type;

    public string Name;
    public string Facing;
    public string Half;
    public string Axis;
    public int GetTextureId(int faceIndex)
    {
        int[] faceTextures = CubeData.blockTextureMap[Name].FaceTextures;
        switch (faceIndex)
        {
            case 0:
                return faceTextures[0];
            case 1:
                return faceTextures[1];
            case 2:
                return faceTextures[2];
            case 3:
                return faceTextures[3];
            case 4:
                return faceTextures[4];
            case 5:
                return faceTextures[5];
            default:
                Debug.Log("Error! Invalid faceIndex.");
                return faceTextures[0];
        }
    }
}
