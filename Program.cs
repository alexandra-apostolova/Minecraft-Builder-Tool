using fNbt;

namespace MinecraftBuilder
{
    public class Program
    {
        static void Main(string[] args)
        {
            NbtFile file = new NbtFile();
            file.LoadFromFile("houseBare.schem");

            NbtCompound root = file.RootTag;
            NbtCompound schematic = root["Schematic"] as NbtCompound;

            short width = schematic.Get<NbtShort>("Width").Value;
            short height = schematic.Get<NbtShort>("Height").Value;
            short length = schematic.Get<NbtShort>("Length").Value;


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
                Console.WriteLine($"{blockToAdd.Type} at {blockToAdd.x} {blockToAdd.y} {blockToAdd.z}");
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



}
