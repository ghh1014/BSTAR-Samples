// ***********************************************************************
// Assembly         : AxisMgnt.dll
// Author           : 
// Created          : 2016-05-30 9:11
// 
// Last Modified By : 郭华华
// Last Modified On : 2016-06-30 17:37
// ***********************************************************************
// <copyright file="AxisGroupViewModel.cs" company="深圳筑星科技有限公司">
//      Copyright (c) BStar All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WallE.Core;

namespace AxisMgntSample.ViewModels.Ribbons
{
    public class AxisGroupViewModel : RibbonGroupViewModel
    {
        public AxisGroupViewModel()
            : base("Ribbons/AxisMgntGroup.xml", "Assets")
        {
            AxisManagement = new RelayCommand(OnAxisManagement, CanAxisManagement);
        }

        private IProjectItemNode GetProjectItemNode()
        {
            var curProjectScene = M.SceneManager.ProjectScene;
            var selectedNodes = curProjectScene.GetSelectedNodes().ToList();
            if (selectedNodes.Count == 0)
            {
                if (M.SceneManager.ProjectScene.ItemNodes.Count(t => t.LoadState == LoadState.Loaded) == 1)
                    return M.SceneManager.ProjectScene.ItemNodes.FirstOrDefault(t => t.LoadState == LoadState.Loaded);
            }
            var projectItemNodes =
                selectedNodes.Select(t => t.AncestorsAndSelf().FirstOrDefault(s => s is IProjectItemNode)).Distinct();
            var proItemNodes = projectItemNodes.Select(t => t as IProjectItemNode).ToList();
            return proItemNodes.Count != 1 ? null : proItemNodes[0];
        }

        #region 轴线管理
        public RelayCommand AxisManagement { get; private set; }

        private bool CanAxisManagement()
        {
            var projectItemNode = GetProjectItemNode();
            if (projectItemNode == null)
                return false;
            return projectItemNode.LoadState != LoadState.Unload;
        }

        private async void OnAxisManagement()
        {
            var projectItemNode = GetProjectItemNode();
            if (projectItemNode == null || projectItemNode.LoadState == LoadState.Unload)
                return;
            var projectItem = projectItemNode.ProjectItem;
            this.ShowMessage($"打开子项目({projectItem.Name})轴线管理", "提示", MessageBoxButton.OK);
            //加载轴线数据
            var axisInfos = await WebDataCenter.Ins.GetAxisInfosAsync(projectItem.Key);
            this.ShowMessage($"轴线的记录数为{axisInfos.Count}", "提示", MessageBoxButton.OK);
            //加载标高数据
            var levelInfos = await WebDataCenter.Ins.GetAxisInfosAsync(projectItem.Key);
            this.ShowMessage($"标高的记录数为{levelInfos.Count}", "提示", MessageBoxButton.OK);
        }
        #endregion
    }
}