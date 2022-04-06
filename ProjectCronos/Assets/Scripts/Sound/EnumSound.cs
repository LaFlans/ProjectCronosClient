using MasterMemory;
using MessagePack;

namespace ProjectCronos
{
    [MemoryTable("m_sound"), MessagePackObject]
    public partial class EnumSound
    {
        [Key(0)]
        [PrimaryKey]
        public int Id { get; set; }
        [Key(1)]
        public string Name { get; set; }
        [Key(2)]
        public string Path { get; set; }

        public override string ToString()
        {
            return "Id, Name, Path" + string.Format("{0}, {1}, {2}",Id, Name, Path);
        }
    }
}
