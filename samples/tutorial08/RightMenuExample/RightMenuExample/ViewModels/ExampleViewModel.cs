using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using RightMenuExample.Model;
using Telerik.Windows;
using WallE.Core;

namespace RightMenuExample.ViewModels
{
    public class ExampleViewModel : ViewModelBase, IHandle<RadRoutedEventArgs>
    {
        #region Private Field

        private List<Models> _dataSource;
        private Models _selectItem;
        private ObservableCollection<MenuItemInfo> _menuSource;
        private MenuHub menuHub;
        string menuName = "ExampleMunuName";
        #endregion

        #region Public Property
        public List<Models> DataSource
        {
            get { return _dataSource; }
            set { Set("DataSource", ref _dataSource, value); }
        }

        public Models SelectItem
        {
            get { return _selectItem; }
            set { Set("SelectItem", ref _selectItem, value); }
        }

        public ObservableCollection<MenuItemInfo> MenuSource
        {
            get { return _menuSource; }
            set { Set("MenuSource", ref _menuSource, value); }
        }
        #endregion

        #region Construction

        public ExampleViewModel()
        {
            _dataSource = new List<Models>()
            {
                new Models() {Colums1 = "a1",Colums2 = "b1",Colums3 = "c1"},
                new Models() {Colums1 = "a2",Colums2 = "b2",Colums3 = "c2"},
                new Models() {Colums1 = "a3",Colums2 = "b3",Colums3 = "c3"},
                new Models() {Colums1 = "a4",Colums2 = "b4",Colums3 = "c4"}
            };
            InitCommand();
            InitMenu();
        }

        #endregion

        #region Command/Event
        public ICommand Menu1Command { get; set; }
        public ICommand Menu2Command { get; set; }
        public ICommand Menu3Command { get; set; }
        public ICommand Menu4Command { get; set; }
        public ICommand AddMenuItem { get; set; }
        public ICommand RemoveMenuItem { get; set; }
        #endregion

        #region Override Method

        #endregion

        #region Private/Public Method

        public void InitCommand()
        {
            Menu1Command = new RelayCommand(OnMenuCommand);
            Menu2Command = new RelayCommand(OnMenuCommand);
            Menu3Command = new RelayCommand(OnMenuCommand);
            Menu4Command = new RelayCommand(OnMenuCommand);
            AddMenuItem = new RelayCommand(OnAddMenuItem);
            RemoveMenuItem = new RelayCommand(OnRemoveMenuItem);
        }

        private void InitMenu()
        {

            menuHub = new MenuHub(menuName);
            _menuSource = new ObservableCollection<MenuItemInfo>();
            MenuItemInfo menu1 = new MenuItemInfo("菜单1", Menu1Command) { GroupName = "Group1" };
            MenuItemInfo menu2 = new MenuItemInfo("菜单2", Menu2Command) { GroupName = "Group1" };
            MenuItemInfo menu3 = new MenuItemInfo("菜单3", Menu3Command) { GroupName = "Group2" };
            MenuItemInfo menu4 = new MenuItemInfo("菜单4", Menu4Command) { GroupName = "Group2" };
            //菜单项还可以通过一下方式定义，CommandName为全局命令中定义的命令名称
            //MenuItemInfo menu4 = new MenuItemInfo("菜单4", "CommandName") { GroupName = "Group2" };
            menu1.SubItems.Add(menu2);
            menu1.SubItems.Add(menu3);
            //向全局注册菜单 
            menuHub.Register(menuName, menu1, 1);
            menuHub.Register(menuName, menu2, 2);
            menuHub.Register(menuName, menu3, 3);
            menuHub.Register(menuName, menu4, 4);

            //_menuSource.Add(menu1);
            //_menuSource.Add(menu2);
            //_menuSource.Add(menu3);
            //_menuSource.Add(menu4);
            //GetMenuItemInfos 为获取合并后的右键菜单
            //_menuSource = menuHub.GetMenuItemInfos(menuNames.Where(t=>t== "menu1"))?.ToList();
        }


        public void Handle(Message<RadRoutedEventArgs> message)
        {
            if (message.Name != "MenuOpening")
                return;
            _menuSource.Clear();
            var list = menuHub.GetMenuItemInfos(new List<string>() { menuName });
            if (list != null)
                _menuSource.AddRange(list);

        }
        private void OnMenuCommand()
        {
            if (SelectItem != null)
                MessageBox.Show(SelectItem.Colums1 + SelectItem.Colums2 + SelectItem.Colums3);
        }

        private void OnAddMenuItem()
        {
            if(_menuSource.Count==0)
                return;
            var menucount = _menuSource.Count(t=>!t.IsSeparator);
            MenuItemInfo menu = new MenuItemInfo("菜单" + (menucount + 1), Menu4Command) { GroupName = "Group" };
            menuHub.Register(menuName, menu);

            //在其他的插件中可以通过一下方式注册菜单
            //M.MenuManager.Register(menuName, menu);
        }
        /// <summary>
        /// 移除菜单项
        /// </summary>
        private void OnRemoveMenuItem()
        {
            if (_menuSource.Count == 0)
                return;
            var menucount = _menuSource.Count(t => !t.IsSeparator);
            MenuItemInfo menu = new MenuItemInfo("菜单" + menucount, Menu4Command) { GroupName = "Group" };
            menuHub.Unregister(menuName, menu);

            //在其他的插件中可以通过一下方式注册菜单
            //M.MenuManager.Unregister(menuName, menu);
        }
        #endregion
    }
}
