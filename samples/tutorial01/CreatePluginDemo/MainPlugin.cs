using WallE.Core;

namespace CreatePluginDemo
{
    public class MainPlugin : IPlugin
    {
        public void Install(IPluginInfo pluginInfo)
        {
            this.ShowMessage("Hello World!");
            //加载插件时，处理的代码
        }

        public void Uninstall()
        {
            //卸载插件时，处理的代码
        }
    }
}
