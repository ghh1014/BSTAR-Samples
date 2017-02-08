using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WallE.Core;
using WallE.Core.Extend;
using WallE.Core.Models;
using WbsExplorer;

namespace AuthorityAndModeExample.ViewModels
{
    public class AuthorityAndModeViewModel : ViewModelBase
    {
        #region Private Field

        private string projectId;

        #endregion

        #region Public Property

        #endregion

        #region Construction

        public AuthorityAndModeViewModel()
        {
            Click=new AsyncCommand(OnClick,CanClick);
            SubClick1=new AsyncCommand(OnClick, CanSubClick1);
            SubClick2 = new AsyncCommand(OnClick, CanSubClick2);
           

        }

        #endregion

        #region Command/Event
        public  AsyncCommand Click { get; set; }

        public void OnClick()
        {
            MessageBox.Show("aaaa");
        }

        private async Task<bool> CanClick()
        {
            var selectProject = GetProjectItem();
            if (selectProject != null)
                projectId = selectProject.Key;
            var result = await M.EnvManager.GetCurrentUserRealPermissionAsync(ModuleKeys.Model, projectId);
            return result > PrivilegeType.NoAccess;
        }
        public  AsyncCommand SubClick1 { get; set; }
        public void OnSubClick1()
        {
        }

        private async Task<bool> CanSubClick1()
        {
            var selectProject = GetProjectItem();
            if (selectProject != null)
                projectId = selectProject.Key;
            var result = await M.EnvManager.GetCurrentUserRealPermissionAsync(ModuleKeys.SubModel1, projectId);
            return result > PrivilegeType.NoAccess;
        }

        public  AsyncCommand SubClick2 { get; set; }
        public void OnSubClick2()
        {
        }

        private async Task<bool> CanSubClick2()
        {
            var selectProject = GetProjectItem();
            if (selectProject != null)
                projectId = selectProject.Key;
            var result = await M.EnvManager.GetCurrentUserRealPermissionAsync(ModuleKeys.SubModel2, projectId);
            return result > PrivilegeType.NoAccess;
        }
        #endregion

        #region Override Method

        #endregion

        #region Private/Public Method
        private ProjectItem GetProjectItem()
        {
            var re = GetProjectItemNode();
            if (re == null || re.LoadState != LoadState.Loaded)
                return null;
            return re.ProjectItem;
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
        #endregion
    }
}
