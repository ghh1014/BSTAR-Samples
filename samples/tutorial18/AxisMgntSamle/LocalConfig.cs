using System.Xml.Linq;
using WallE.Core;

namespace AxisMgntSample
{
    public static class LocalConfig
    {
        static LocalConfig()
        {
            var root = XDocument.Load(typeof(LocalConfig).GetPluginResPath("__config.xml")).Root;
            InsertTabName = root.Attribute("InsertTabName").Value;
        }

        public static string InsertTabName { get; }
    }
}
