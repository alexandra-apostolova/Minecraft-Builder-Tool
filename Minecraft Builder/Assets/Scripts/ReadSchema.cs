using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using static Unity.Collections.AllocatorManager;
using fNbt;
using System.IO;
public class ReadSchema : MonoBehaviour
{
    public short width;
    public short height;
    public short length;
    void Start()
    {
        NbtFile file = new NbtFile();
        file.LoadFromFile("houseBare.schem");

        NbtCompound root = file.RootTag;
        NbtCompound schematic = root["Schematic"] as NbtCompound;

        width = schematic.Get<NbtShort>("Width").Value;
        height = schematic.Get<NbtShort>("Height").Value;
        length = schematic.Get<NbtShort>("Length").Value;

        NbtCompound blocks = schematic.Get<NbtCompound>("Blocks");
        NbtCompound paletteTag = blocks.Get<NbtCompound>("Palette");

        Dictionary<int, string> palette = new Dictionary<int, string>();
        foreach (var tag in paletteTag.Tags)
        {
            int id = tag.IntValue;
            string blockName = tag.Name;

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

            (int x, int y, int z) = IndexToPos(i, width, length);

            Block blockToAdd = new Block
            {
                x = x,
                y = y,
                z = z,
                Type = palette[paletteIndex]
            };
            blocksList.Add(blockToAdd);
        }
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
