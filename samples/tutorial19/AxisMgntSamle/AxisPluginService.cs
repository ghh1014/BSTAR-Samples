using System;
using System.Collections.Generic;
using System.Linq;
using AxisMgntSample.ViewModels;
using AxisMgntSample.ViewModels.Ribbons;
using WallE.Core;
using WallE.Core.Extend;
using WallE.Core.Models;

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
            M.DockingManager.AddPaneCloseHandler(Ins_PaneClose);
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
            M.DockingManager.RemovePaneCloseHandler(Ins_PaneClose);
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

        /// <summary>
        /// 打开轴线管理面板
        /// </summary>
        /// <param name="parameter"></param>
        public void OpenAxisMgnt(Object parameter)
        {
            var proItem = parameter as ProjectItem;
            if (proItem == null)
                return;
            //判断是否已经存在面板
            if (_axisPaneVmsDic.ContainsKey(proItem.Key))
            {
                var existVm = _axisPaneVmsDic[proItem.Key];
                if (!existVm.IsActive)
                    existVm.IsActive = true;
                return;
            }
            var paneVm = new DockingPaneViewModel(new AxisSpaceMgntViewModel(proItem))
            {
                Header = proItem.Name + "：轴线管理示例",
                IsDocument = true
            };
            _axisPaneVmsDic.Add(proItem.Key, paneVm);
            M.DockingManager.InsertPane(paneVm);
        }

        private void Ins_PaneClose(object sender, PaneStateChangeEventArgs e)
        {
            CloseChanged(_axisPaneVmsDic, e.PaneVms);
        }

        public static void CloseChanged(Dictionary<string, DockingPaneViewModel> vmDic, IEnumerable<DockingPaneViewModel> panVms)
        {
            if (vmDic == null)
                return;
            foreach (var dockingPaneViewModel in panVms)
            {
                var keys = vmDic.Keys.ToList();
                foreach (var curKey in keys)
                {
                    if (vmDic[curKey] == dockingPaneViewModel)
                        vmDic.Remove(curKey);
                }
            }
        }

        public void CloseDockingPane(string proItemKey)
        {
            if (_axisPaneVmsDic != null && _axisPaneVmsDic.ContainsKey(proItemKey))
            {
                var vm = _axisPaneVmsDic[proItemKey];
                M.DockingManager.RemovePane(vm);
                _axisPaneVmsDic.Remove(proItemKey);
            }
        }

        public void UpdateHeaderStatus(string proItemKey, bool isDrity)
        {
            if (_axisPaneVmsDic != null && _axisPaneVmsDic.ContainsKey(proItemKey))
            {
                var vm = _axisPaneVmsDic[proItemKey];
                var flag = vm.Header.EndsWith(" *");
                if (flag == isDrity)
                    return;
                if (isDrity)
                    vm.Header += " *";
                else
                    vm.Header = vm.Header.Substring(0, vm.Header.Length - 2);
            }
        }
    }
}
