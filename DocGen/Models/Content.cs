namespace DocGen.Models
{
    internal class Content
    {
        public string content { get; set; }

        public Content()
        {
            this.content = string.Empty;
        }

        public Content(string content)
        {
            this.content = content;
        }
    }
}
