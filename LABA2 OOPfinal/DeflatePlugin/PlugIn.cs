using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.IO;
using System.IO.Compression;

namespace DeflatePlugin
{
    public class PlugIn : IPlugin
    {
        public string PluginName { get { return "DefLate-архивация"; } }
        public string PluginExtention { get { return ".zlib"; } }
        public string PluginExtentionName { get { return ".zlib |*.zlib"; } }

        public void OpenFile(string inputFileName, string outputFileName)
        {
            using (FileStream sourceStream = new FileStream(inputFileName, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(outputFileName))
                {
                    using (DeflateStream decompressionStream = new DeflateStream(sourceStream, CompressionMode.Decompress))
                    {
                        decompressionStream.CopyTo(targetStream);
                    }
                }
            }
        }

        public void SaveFile(string inputFileName)
        {
            string OutputFileName = inputFileName + PluginExtention;
            using (FileStream sourceStream = new FileStream(inputFileName, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(OutputFileName))
                {
                    using (DeflateStream compressionStream = new DeflateStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);
                    }
                }
            }
            File.Delete(inputFileName);
        }
    }
}
