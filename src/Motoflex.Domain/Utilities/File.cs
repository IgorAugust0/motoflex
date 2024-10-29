namespace Motoflex.Domain.Utilities
{
    public class File(Stream stream, string name, string type)
    {
        public Stream Stream { get; } = stream;
        public string Name { get; set; } = name;
        public string Type { get; } = type;
    }
}
