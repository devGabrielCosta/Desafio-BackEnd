namespace Domain.Utilities
{
    public class File
    {
        public Stream Stream { get; }
        public string Name { get; set; }
        public string Type { get; }
        public File(Stream stream, string name, string type)
        {
            Stream = stream;
            Name = name;
            this.Type = type;
        }
    }
}
