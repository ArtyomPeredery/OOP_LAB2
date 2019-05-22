using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.IO;
using System.IO.Compression;

namespace CipherPlugin
{
    public class PlugIn : IPlugin
    {
        public string PluginName { get { return "zip-архивация"; } }
        public string PluginExtention { get { return ".zip"; } }
        public string PluginExtentionName { get { return ".zip |*.zip"; } }

        public void OpenFile(string inputFileName, string outputFileName)
        {
            ZipFile.ExtractToDirectory(inputFileName, Path.GetDirectoryName(outputFileName));
        }
        public void SaveFile(string inputFileName)
        {
            string OutputFileName = inputFileName + PluginExtention;
            using (FileStream input = File.OpenRead(inputFileName))
            using (FileStream output = File.Create(OutputFileName))
            using (ZipArchive zip = new ZipArchive(output, ZipArchiveMode.Create))
            {
                ZipArchiveEntry entry = zip.CreateEntry(Path.GetFileName(inputFileName));
                using (Stream stream = entry.Open())
                {
                    input.CopyTo(stream);
                }
            }
            File.Delete(inputFileName);
        }

        
    }
}
