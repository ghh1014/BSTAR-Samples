using RightMenuExample.ViewModels;
using WallE.Core;
namespace CommandExample
{
    public class MainPlugin : IPlugin
    {
        private DockingPaneViewModel _panel;
        public void Install(IPluginInfo pluginInfo)
        {
            ExampleViewModel examleViewModel = new ExampleViewModel();
            _panel = new DockingPaneViewModel(examleViewModel)
            {
                Header = "右键菜单测试示例",
                IsActive = true,
                IsHidden = false,
                InitialPosition = DockPosition.FloatingDockable,
                IsDocument = true
            };
            M.DockingManager.InsertPane(_panel);
        }

        public void Uninstall()
        {
        }
    }
}
