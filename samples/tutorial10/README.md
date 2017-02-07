# 使用Telerik控件库

想要做出更炫更厉害的效果，除了好好自己写控价之外，还有一个捷径。那就是，使用别人已经完善好的控件库，比如，Telerik，下面我们来学习，怎么样在`BIM-STAR`平台下使用Telerik。

> Telerik控件，UI for WPF 官方网站：http://www.telerik.com/products/wpf/overview.aspx，请下载Telerik库。

- 添加引用`Telerik.Windows.Controls.dll`、`Telerik.Windows.Data.dll`和`Telerik.Windows.Controls.Input.dll`
- 添加`Telerik`基本控件

比如添加一个`telerik:RadComboBox`控件。

`View`层，前端添加代码，如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="RadComboBox控件：" VerticalAlignment="Center" HorizontalAlignment="Right" />
    <telerik:RadComboBox Grid.Column="1" VerticalAlignment="Center" SelectedValuePath="Item1" DisplayMemberPath="Item2" ItemsSource="{Binding YearList}" x:Name="RCbxYear" MinWidth="150" />
</Grid>
```

`ViewModel`层，添加代码，如下

```c#
using System;
using System.Collections.Generic;
using WallE.Core;

namespace ControlsDemo.ViewModels
{
    public class DemoCreateViewModel : ViewModelBase
    {
        public DemoCreateViewModel()
        {
            YearList = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("2017", "2017年"),
                new Tuple<string, string>("2018", "2018年"),
                new Tuple<string, string>("2019", "2019年")
            };
        }

        private List<Tuple<string, string>> _yearList;

        /// <summary>
        /// 年份列表
        /// </summary>
        public List<Tuple<string, string>> YearList
        {
            get { return _yearList; }
            set { Set("YearList", ref _yearList, value); }
        }
    }
}
```

其中`YearList`为`ViewModel`层数据获取，供`View`层显示。这样一个`Telerik`的`RadCombox`控件就可以使用了。

> 添加以上两个`dll`文件后，可以创建`Telerik`基本控件。如果想使用更复杂一点的控件，需要引入相应的`dll`,比如：表格，下面对这个进行说明。

添加`telerik:RadGridView`控件为例，需要添加引用`Telerik.Windows.Controls.GridView.dll`。

`View`层，前端添加代码，如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="RadGridView控件：" VerticalAlignment="Top" HorizontalAlignment="Right" />
    <telerik:RadGridView Grid.Column="1" AutoGenerateColumns="False" SelectionMode="Single" ShowGroupPanel="False" IsFilteringAllowed="False"
                 RowIndicatorVisibility="Collapsed" CanUserFreezeColumns="False" IsReadOnly="True" ShowColumnSortIndexes="True"
                 ItemsSource="{Binding UserSource}" SelectedItem="{Binding UserItem}">
        <telerik:RadGridView.Columns>
            <telerik:GridViewDataColumn Header="序号" DataMemberBinding="{Binding Number}" />
            <telerik:GridViewDataColumn Header="姓名" DataMemberBinding="{Binding Name}" />
            <telerik:GridViewDataColumn Header="备注" DataMemberBinding="{Binding Remark}" />
        </telerik:RadGridView.Columns>
    </telerik:RadGridView>
</Grid>
```

其中，`ItemsSource="{Binding UserSource}"`为列表绑定的数据源。在插件根目录下，创建`Models`文件夹，新建一个`User`类，作为数据源Model类，如下：

```c#
using WallE.Core;

namespace ControlsDemo.Models
{
    public class User : NotifyObject
    {
        private string _number;

        /// <summary>
        /// 获取或设置Number属性
        /// </summary>
        public string Number
        {
            get { return _number; }
            set { Set("Number", ref _number, value); }
        }

        private string _name;

        /// <summary>
        /// 获取或设置Name属性
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        private string _remark;

        /// <summary>
        /// 获取或设置Remark属性
        /// </summary>
        public string Remark
        {
            get { return _remark; }
            set { Set("Remark", ref _remark, value); }
        }
    }
}
```

`ViewModel`层，添加代码，如下：

```c#
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using ControlsDemo.Models;
using WallE.Core;

namespace ControlsDemo.ViewModels
{
    public class DemoCreateViewModel : ViewModelBase
    {
        public DemoCreateViewModel()
        {
            YearList = new List<Tuple<string, string>>
            {
                new Tuple<string, string>("2017", "2017年"),
                new Tuple<string, string>("2018", "2018年"),
                new Tuple<string, string>("2019", "2019年")
            };
            UserSource = new ObservableCollection<User>
            {
                new User{Number = "1",Name="小茗",Remark="-"},
                new User{Number = "2",Name="冷冷",Remark="-"},
                new User{Number = "3",Name="暖暖",Remark="-"}
            };
        }

        private List<Tuple<string, string>> _yearList;

        /// <summary>
        /// 年份列表
        /// </summary>
        public List<Tuple<string, string>> YearList
        {
            get { return _yearList; }
            set { Set("YearList", ref _yearList, value); }
        }

        private ObservableCollection<User> _userSource;

        /// <summary>
        /// 获取或设置UserSource属性
        /// </summary>
        public ObservableCollection<User> UserSource
        {
            get { return _userSource; }
            set { Set("UserSource", ref _userSource, value); }
        }

        private User _userItem;

        /// <summary>
        /// 获取或设置UserItem属性
        /// </summary>
        public User UserItem
        {
            get { return _userItem; }
            set { Set("UserItem", ref _userItem, value); }
        }
    }
}
```

其中`UserSource`为列表数据源，供`View`层显示数据，完成了控件数据绑定，详细的数据绑定后边会有介绍。这样，表格控件就引入进来了。`Telerik`还有很多其他的控件，按照这个实例添加就可以啦。淡然，也会遇到一些问题，建议多去`Telerik`官网查看资料。