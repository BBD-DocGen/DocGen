using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DocGen.Classes
{
    internal class GetFileViaDialog
    {
        private OpenFileDialog FileDialog { get; set; }

        public GetFileViaDialog ()
        {
            FileDialog = new OpenFileDialog();
            FileDialog.Filter = "cs files (*.cs)|*.cs|All files (*.*)|*.*";
        }

        public string getFileContents()
        {
            FileDialog.ShowDialog();

            try
            {
                Stream fileStream = FileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return reader.ReadToEnd();
                }
            } 
            catch (InvalidOperationException ex)
            {
                return $"Invalid operation {ex.Message}";
            }
            catch (Exception ex)
            {
                return $"I don't know what happened {ex.Message}";
            }
        }
    }
}
