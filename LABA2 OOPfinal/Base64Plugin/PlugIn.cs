using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Plugin;
using System.IO;
using System.IO.Compression;

namespace Base64Plugin
{
    public class PlugIn : IPlugin
    {
        public string PluginName { get { return "Base64-кодирование"; } }
        public string PluginExtention { get { return ".rtf"; } }
        public string PluginExtentionName { get { return ".rtf|*.rtf"; } }

        public void OpenFile(string inputFileName, string outputFileName)
        {
            string base64String;
            using (StreamReader sourceStream = new StreamReader(new FileStream(inputFileName, FileMode.OpenOrCreate)))
            {
                base64String = sourceStream.ReadToEnd();
            }
            var decoded = Convert.FromBase64String(base64String);
            using (var streamWriter = new BinaryWriter(new FileStream(outputFileName, FileMode.OpenOrCreate)))
            {
                streamWriter.Write(decoded);
            }          
        }
   
        public void SaveFile(string inputFileName)
        {
            string OutputFileName = inputFileName + PluginExtention;
            byte[] bytes = File.ReadAllBytes(inputFileName);
            string newValue = Convert.ToBase64String(bytes);
            using (var streamWriter = new StreamWriter(OutputFileName))
            {
                streamWriter.Write(newValue);
            }
            File.Delete(inputFileName);
        }       
    }
}
