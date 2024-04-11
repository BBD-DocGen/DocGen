namespace DocGen.Models
{
    internal class GeneratedDocs
    {
        public int GenDocID { get; set; }
        public string GenDocName { get; set; }
        public string GenDocURL { get; set; }

        public GeneratedDocs()
        {
            GenDocName = string.Empty;
            GenDocURL = string.Empty;
        }

        public GeneratedDocs(int genDocID, string genDocName, string genDocURL)
        {
            GenDocID = genDocID;
            GenDocName = genDocName;
            GenDocURL = genDocURL;
        }
    }
}
