using CreatePluginDemo.ViewModels;
using CreatePluginDemo.Views;
using WallE.Core;

namespace CreatePluginDemo
{
    public class MainPlugin : IPlugin
    {
        private DockingPaneViewModel _dockingVm;
        private FirstViewModel _firstVm;
        public void Install(IPluginInfo pluginInfo)
        {
            if (GlobalConfig.Ins.AlwaysShow)
            {
                _firstVm = new FirstViewModel();
                _dockingVm = new DockingPaneViewModel(_firstVm)
                {
                    Header = "面板的标题",
                };
                M.DockingManager.InsertPane(_dockingVm);
            }
            //系统设置中插件的设置界面
            M.SettingManager.AddPluginSettingItem("MyPluginSetting", new SettingItem("我的插件", new SettingView()));
        }

        public void Uninstall()
        {
            //关闭面板
            if (_dockingVm != null)
                M.DockingManager.RemovePane(_dockingVm);
            _dockingVm = null;
            //移除设置界面
            M.SettingManager.RemovePluginSettingItem("MyPluginSetting");
        }
    }
}
