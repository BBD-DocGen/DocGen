using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Models
{
    internal class FileModel
    {
        public string FileName { get; set; }
        public string FileContents { get; set; }
        public string FilePath { get; set; }
        public string FileSummary { get; set; }

        public FileModel() 
        { 
            this.FileName = string.Empty;
            this.FileContents = string.Empty;
            this.FileSummary = string.Empty;
            this.FileSummary = string.Empty;
        }

        public FileModel(string fileName, string fileContents, string filePath, string fileSummary)
        {
            FileName = fileName;
            FileContents = fileContents;
            FilePath = filePath;
            FileSummary = fileSummary;
        }
    }
}
