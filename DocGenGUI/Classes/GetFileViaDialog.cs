using DocGen.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

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

        public FileModel? getFileContents()
        {
            FileDialog.ShowDialog();

            try
            {
                Stream fileStream = FileDialog.OpenFile();

                using (StreamReader reader = new StreamReader(fileStream))
                {
                    return new FileModel
                    {
                        FileName = Path.GetFileNameWithoutExtension(FileDialog.FileName),
                        FileContents = reader.ReadToEnd(),
                        FilePath = FileDialog.FileName
                    };
                }
            } 
            catch (InvalidOperationException ex)
            {
                MessageBox.Show($"Invalid operation {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"I don't know what happened {ex.Message}");
            }

            return null;
        }
    }
}
