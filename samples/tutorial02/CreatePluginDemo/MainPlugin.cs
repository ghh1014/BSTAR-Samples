using CreatePluginDemo.ViewModels;
using WallE.Core;

namespace CreatePluginDemo
{
    public class MainPlugin : IPlugin
    {
        private RibbonGroupViewModel _groupVm;
        private DockingPaneViewModel _dockingVm;
        private FirstViewModel _firstVm;
        private RibbonButtonViewModel _viewBtn;
        public void Install(IPluginInfo pluginInfo)
        {
            _groupVm = M.RibbonManager.GetRibbonGroup("BasicViewGroup");//获取视图菜单中的基本菜单组
            if (_groupVm != null)
            {
                _viewBtn = new RibbonButtonViewModel
                {
                    Text = "展示HelloWorld面板",
                    Click = new RelayCommand(ShowPane)//按钮命令
                };
                _groupVm.Items.Add(_viewBtn);//在基本菜单组里添加一个按钮
            }
            else
                this.ShowMessage("没有找到名称为ViewGroup的RibbonGroup！漫游管理器插件无法插入相关Ribbon菜单！");
        }

        private void ShowPane()
        {
            if (_dockingVm != null)
            {
                _dockingVm.IsActive = true;
                _dockingVm.IsHidden = false;
            }
            else
            {
                _firstVm = new FirstViewModel();
                _dockingVm = new DockingPaneViewModel(_firstVm)
                {
                    Header = "面板的标题",
                };
                M.DockingManager.InsertPane(_dockingVm);
            }
        }

        public void Uninstall()
        {
            //移除相关菜单按钮
            if (_groupVm != null)
                _groupVm.Items.Remove(_viewBtn);
            //关闭面板
            if (_dockingVm != null)
                M.DockingManager.RemovePane(_dockingVm);
            _dockingVm = null;
        }
    }
}
