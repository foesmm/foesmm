namespace FoESMM.Common
{
    public class PluginFactory
    {
        public Plugin ReadFile(string sFileName)
        {
            return ReadFile(sFileName, false);
        }

        public Plugin ReadFile(string sFileName, bool bMaster)
        {
            return new Plugin(sFileName);
        }
    }
}