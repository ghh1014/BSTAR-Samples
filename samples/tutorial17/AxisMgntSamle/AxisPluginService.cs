using System.Collections.Generic;
using AxisMgntSample.ViewModels.Ribbons;
using WallE.Core;
using WallE.Core.Extend;

namespace AxisMgntSample
{
    public class AxisPluginService
    {
        private static AxisPluginService _ins;

        private AxisPluginService() { }

        /// <summary>
        /// 全局唯一变量
        /// </summary>
        public static AxisPluginService Ins
        {
            get { return _ins ?? (_ins = new AxisPluginService()); }
        }

        private RibbonTabViewModel _mgntTabVm;
        private AxisGroupViewModel _axisGroupVm;
        private Dictionary<string, DockingPaneViewModel> _axisPaneVmsDic;

        public void Init()
        {
            _axisGroupVm = new AxisGroupViewModel();
            //初始化集合
            _axisPaneVmsDic = new Dictionary<string, DockingPaneViewModel>();
            //插入Ribbon菜单
            _mgntTabVm = M.RibbonManager.GetRibbonTab(LocalConfig.InsertTabName);
            if (_mgntTabVm != null)
                _mgntTabVm.Groups.Add(_axisGroupVm);
            else
                LoggerHelper.Debug($"未找到名称为{LocalConfig.InsertTabName}的RibbonTab，无法插入轴线管理相关的Ribbon菜单！");
            //插件设置界面......
            //注册相关命令
            M.CommandManager.Register(new CommandInfo("AxisManagement", _axisGroupVm.AxisManagement));
            //注册右键菜单......
        }

        public void Dispose()
        {
            if (_mgntTabVm == null)
                return;
            //取消注册相关命令
            M.CommandManager.Unregister("AxisManagement");
            _mgntTabVm.Groups.Remove(_axisGroupVm);
            _axisGroupVm.Dispose();
            //移除面板
            foreach (var dbVm in _axisPaneVmsDic.Values)
                M.DockingManager.RemovePane(dbVm);
            _axisPaneVmsDic.Clear();
            _axisPaneVmsDic = null;
            //移除设置界面......
            //移除右键菜单......
        }
    }
}
