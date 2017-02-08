using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WallE.Core;

namespace Ribbon
{
    public class RibbonExample
    {
        public RibbonTabViewModel GetRibbonTabViewModel()
        {
            return M.RibbonManager.GetRibbonTab("TanName");
        }

        /// <summary>
        /// 直接通过名称获取
        /// </summary>
        /// <returns></returns>
        public RibbonGroupViewModel GetButtonGroupViewModel()
        {
            return M.RibbonManager.GetRibbonGroup("GroupName");
        }
        /// <summary>
        /// 通过Tab标签获取Tab下的Groups
        /// </summary>
        /// <returns></returns>
        public IEnumerable<RibbonGroupViewModel> GetButtonGroupViewModels()
        {
            return M.RibbonManager.GetRibbonTab("TanName").Groups;
        }
        /// <summary>
        /// 创建Tab
        /// </summary>
        public static RibbonTabViewModel CreateRibbonTab()
        {
            var tab = new RibbonTabViewModel()
            {
                Name = "ExampleTab",
                Header = "测试Tab",
            };
            M.RibbonManager.InsertRibbonTab(tab, 1);
            return tab;
        }
        /// <summary>
        /// 创建Group
        /// </summary>
        public static RibbonGroupViewModel CreateRibbonGroup()
        {
            RibbonGroupViewModel group=new RibbonGroupViewModel()
            {
                Name = "ExampleGroup",
                Header = "测试Group",               
            };
            return group;
        }

        public static RibbonButtonViewModel CreateRibbonButton()
        {
            RibbonButtonViewModel buttonVm = new RibbonButtonViewModel()
            {
                Name = "Name",
                Text = "测试Button",
                LargeImage = "url",
                ButtonSize = ButtonSize.Large,
            };
            return buttonVm;
        }

        /// <summary>
        /// Group加入Tab
        /// </summary>
        public void JoinRibbonTab()
        {
            var tab = M.RibbonManager.GetRibbonTab("TabName");
            tab.Groups.Insert(0,new RibbonGroupViewModel()
            {
                Name = "Name",
                Header = "Header",
            });
        }

        public void JoinRibbonGroup()
        {
            var tab = M.RibbonManager.GetRibbonGroup("GroupName");
            tab.Items.Insert(0, new RibbonButtonViewModel()
            {
                Name = "Name",
                Text = "Text",
                LargeImage = "url",
                ButtonSize = ButtonSize.Large,
                Click = new RelayCommand(OnClick)
            });
        }

        public void RemoveRibbonTab()
        {
            var tab = M.RibbonManager.GetRibbonTab("TabName");
            M.RibbonManager.RemoveRibbonTab(tab);
        }
        public void RemoveRibbonGroup()
        {
            var tab = M.RibbonManager.GetRibbonTab("TabName");
            var group = M.RibbonManager.GetRibbonGroup("GroupName");
            tab.Groups.Remove(group);
        }
        //public void RemoveRibbonButton()
        //{
        //    var group = M.RibbonManager.GetRibbonGroup("GroupName");
        //    group.Items.Remove(buttonVm);
        //}
        private void OnClick()
        {
        }
    }
}
