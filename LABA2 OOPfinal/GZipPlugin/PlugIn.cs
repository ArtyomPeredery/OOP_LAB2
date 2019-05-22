using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.IO;
using System.IO.Compression;

namespace GZipPlugin
{
    public class PlugIn : IPlugin
    {
        public string PluginName { get { return "Gzip-архивация"; } }
        public string PluginExtention { get { return ".gz"; } }
        public string PluginExtentionName { get { return ".gz |*.gz"; } }

        public void OpenFile(string inputFileName, string outputFileName)
        {
            using (FileStream sourceStream = new FileStream(inputFileName, FileMode.OpenOrCreate))
            {
                using (FileStream targetStream = File.Create(outputFileName))
                {
                    using (GZipStream decompressionStream = new GZipStream(sourceStream, CompressionMode.Decompress))
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
                    using (GZipStream compressionStream = new GZipStream(targetStream, CompressionMode.Compress))
                    {
                        sourceStream.CopyTo(compressionStream);                    
                    }
                }
            }
            File.Delete(inputFileName);
        }

    }
}
