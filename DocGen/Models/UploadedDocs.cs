using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Models
{
    internal class UploadedDocs
    {
        public int UpDocID { get; set; }
        public string UpDocName { get; set; }
        public string UpDocURL { get; set; }

        public UploadedDocs()
        {
            UpDocName = string.Empty;
            UpDocURL = string.Empty;
        }

        public UploadedDocs(int upDocID, string upDocName, string upDocURL)
        {
            UpDocID = upDocID;
            UpDocName = upDocName;
            UpDocURL = upDocURL;
        }
    }
}
