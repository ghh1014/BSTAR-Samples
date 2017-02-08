using Ribbon.ViewModels;
using WallE.Core;

namespace Ribbon
{
    public class MainPlugin : IPlugin
    {
        private RibbonTabViewModel _tab;
        private RibbonTabViewModel _existtab;
        private RibbonGroupViewModel _group;
        private RibbonGroupViewModel _existGroup;
        private RibbonButtonViewModel _button;
        public void Install(IPluginInfo pluginInfo)
        {
            /*******************采用代码方式创建************************/
            //创建RibbonTab标签
            //_tab = RibbonExample.CreateRibbonTab();
            ////创建RibbonGroup 并加入Tab
            //_group = RibbonExample.CreateRibbonGroup();
            //_tab.Groups.Add(_group);
            ////tab.Groups.Insert(0,group);
            ////创建RibbonButton 在上述创建的RibbonGroup中加入RibbonButton
            //_button = RibbonExample.CreateRibbonButton();
            //_button.Click = new RelayCommand(OnClick);//制定button点击的事件
            //_group.Items.Add(_button);

            ////已有的RibbonTab中加入RibbonGroup
            //_existtab = M.RibbonManager.GetRibbonTab("ExampleTab");
            //_existtab.Groups.Add(new RibbonGroupViewModel()
            //{
            //    Name = "ExampleGroup1",
            //    Header = "测试Group1"
            //});//注意如果该Group中没有Item则在界面上不能显示

            ////已有的RibbonGroup中加入RibbonButton
            //_existGroup = M.RibbonManager.GetRibbonGroup("ExampleGroup1");
            //_existGroup.Items.Add(new RibbonButtonViewModel()
            //{
            //    Name = "Name",
            //    Text = "测试Button",
            //    LargeImage = "url",
            //    ButtonSize = ButtonSize.Large,
            //    Click = new RelayCommand(OnClick)
            //});


            /*******************采用Xml方式创建************************/
            M.RibbonManager.InsertRibbonTab(new ViewTabViewModel(), 1);

            //已有的RibbonTab中加入RibbonGroup
            _existtab = M.RibbonManager.GetRibbonTab(LocalConfig.InsertTabName);
            _existtab.Groups.Add(new ViewGroupViewModel());

            //已有的RibbonGroup中加入RibbonButton 

            ViewButtonViewModel buttonGroup=new ViewButtonViewModel();
            //注意，如果向已知的RibbonGroup中添加RibbonButton 建议xml配置文件的外层直接加上group标签 如ViewButton.xml
            //通过继承RibbonGroupViewModel 来解析配置文件得到其中的button 
            _existGroup = M.RibbonManager.GetRibbonGroup(buttonGroup.Name);
           _existGroup.Items.AddRange(buttonGroup.Items); 
        }

        public void Uninstall()
        {
            if (_group != null)
                _group.Items.Remove(_button);//移除RibbonButton
            if (_tab != null)
                _tab.Groups.Remove(_group);//移除RibbonGroup
            M.RibbonManager.RemoveRibbonTab(_tab);//移除RibbonTab
            
        }

        private void OnClick()
        {
        }
    }
}
