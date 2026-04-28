using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
using fNbt;
using System.IO;
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

            (int x, int y, int z) = IndexToPos(i, HouseData.chunkWidth, HouseData.chunkLength);

            Block blockToAdd = new Block
            {
                x = x,
                y = y,
                z = z,
                Type = palette[paletteIndex]
            };

            if (blockToAdd.Type != "air")
            {
                blocksList.Add(blockToAdd);
            }
        }

        HouseData.Blocks = blocksList;
    }
    static (int x, int z, int y) IndexToPos(int index, int width, int length)
    {
        int x = index % width;
        int z = (index / width) % length;
        int y = index / (width * length);

        return (x, y, z);
    }
}

public class Block
{
    public int x;
    public int z;
    public int y;
    public string Type;
}
