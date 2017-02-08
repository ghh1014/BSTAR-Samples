using System.Xml.Linq;
using WallE.Core;

namespace Ribbon
{

    public static class LocalConfig
    {
        static LocalConfig()
        {
            var root = XDocument.Load(typeof(LocalConfig).GetPluginResPath("_config.xml")).Root;
            InsertTabName = root.Element("InsertTabName").Value;
            //InsertGroupName = root.Element("InsertGroupName").Value;
        }
        public static string InsertTabName { get; }
        //public static string InsertGroupName { get; }
    }
}