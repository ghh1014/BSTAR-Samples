using AuthorityAndModeExample.ViewModels;
using WallE.Core;

namespace AuthorityAndModeExample
{
    public class MainPlugin : IPlugin
    {
        private DockingPaneViewModel paneViewModel;
        public void Install(IPluginInfo pluginInfo)
        {
            AuthorityAndModeViewModel authorityAndModeViewModel = new AuthorityAndModeViewModel();
            paneViewModel = new DockingPaneViewModel(authorityAndModeViewModel)
            {
                Header = "权限及模块测试示",
                IsActive = true,
                InitialPosition = DockPosition.FloatingDockable,
                IsHidden = false,
                IsDocument = true
            };
            M.DockingManager.InsertPane(paneViewModel);
        }

        public void Uninstall()
        {
            if (paneViewModel != null)
                M.DockingManager.RemovePane(paneViewModel);
        }


    }
}
