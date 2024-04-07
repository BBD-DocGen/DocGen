using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DocGen.Models
{
    internal class FileModel
    {
        public string? FileName { get; set; }
        public string? FileContents { get; set; }
        public string? FilePath { get; set; }
        public string? FileSummary { get; set; }
    }
}
