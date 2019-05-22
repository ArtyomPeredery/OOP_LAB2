



namespace Plugin
{
    public interface IPlugin
    {
        string PluginName { get; }
        string PluginExtention { get; }
        string PluginExtentionName { get; }
        void OpenFile(string inputFileName, string outputFileName);
        void SaveFile(string inputFileName);
    }
}
