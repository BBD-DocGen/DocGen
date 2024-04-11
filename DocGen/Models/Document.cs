namespace DocGen.Models
{
    internal class Document
    {
        public string fileName { get; set; }
        public string content { get; set; }

        public Document()
        {
            fileName = string.Empty;
            content = string.Empty;
        }

        public Document(string fileName, string content)
        {
            this.fileName = fileName;
            this.content = content;
        }
    }
}
