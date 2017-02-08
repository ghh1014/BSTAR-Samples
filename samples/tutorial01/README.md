## ViewModel与View的创建

对MVVM框架中，创建ViewModel与View是基础工作。下面将一步一步讲述如何在BIM-STAR框架之下如何创建他们。

> 创建的前提是，你已经懂得了如何在BIM-STAR框架下创建插件；然后在此基础之上进行下面的操作，之后就可以看到做的效果

- 第一步 添加引用`WallE.Core.dll`

在插件中添加引用`WallE.Core.dll`，所在目录为：`libs\WallE`

- 第二步 创建文件夹`ViewModels`、`Views`

在插件根目录下创建文件夹`ViewModels`、`Views`。其中`ViewModels`为`ViewModel`层，`Views`为`View`层

- 第三步 在`ViewModels`文件夹下创建`xxxViewModel.cs`类

`xxxViewModel`名称为用户自定义，然后`DemoCreateViewModel`类引用`ViewModelBase`类，参考代码如下：

```c#
using WallE.Core;

namespace ControlsDemo.ViewModels
{
    public class DemoCreateViewModel : ViewModelBase
    {
    }
}
```

- 第四步 在`Views`文件夹下创建`xxxView.xaml`

创建用户控件，其中名称应与想要关联 `ViewModel`层中类的名称一致，如与`DemoCreateViewModel.cs`对应，应命名为`DemoCreateView.xaml`。后端参考代码，如下：

```c#
namespace ControlsDemo.Views
{
    /// <summary>
    /// DemoCreateView.xaml 的交互逻辑
    /// </summary>
    public partial class DemoCreateView
    {
        public DemoCreateView()
        {
            InitializeComponent();
        }
    }
}
```

- 第五步 添加引用`System.Xaml`

支持解析和处理可扩展应用程序标记语言 (XAML)，需要引用`Syatem.Xaml`，默认没有添加，需要自己手动添加。

前端代码示例，如下：

```xml
<UserControl x:Class="ControlsDemo.Views.DemoCreateView"
             xmlns="http://schemas.microsoft.com/winfx/2006/xaml/presentation"
             xmlns:x="http://schemas.microsoft.com/winfx/2006/xaml"
             xmlns:mc="http://schemas.openxmlformats.org/markup-compatibility/2006"
             xmlns:d="http://schemas.microsoft.com/expression/blend/2008"
             mc:Ignorable="d"
             d:DesignHeight="300" d:DesignWidth="300">
     <StackPanel>
            <Grid Margin="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>
                <TextBlock Text="TextBlock控件：" VerticalAlignment="Center" />
                <TextBlock Grid.Column="1" VerticalAlignment="Center">ViewModel及View创建</TextBlock>
            </Grid>
            <Grid Margin="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>
                <TextBlock Text="Text控件：" VerticalAlignment="Center" />
                <TextBox Grid.Column="1" />
            </Grid>
            <Grid Margin="10">
                <Grid.ColumnDefinitions>
                    <ColumnDefinition Width="100" />
                    <ColumnDefinition Width="Auto" />
                </Grid.ColumnDefinitions>
                <TextBlock Text="Button控件：" VerticalAlignment="Center" />
                <Button Grid.Column="1" VerticalAlignment="Center" Content="点我" Width="75" />
            </Grid>
        </StackPanel>
</UserControl> 
```

> 想看效果吗？以上这些还不可以，需要稍稍在写几行代码。还记得插件创建时候的`MainPlugin.cs么？

- 第六步 效果查看

`MainPlugin.cs`中写几行代码，仅仅为了看WPF控件的显示，不做实际开发参考哦。参考代码如下：

```c#
using System.Collections.Generic;
using ControlsDemo.ViewModels;
using WallE.Core;

namespace ControlsDemo
{
    public class MainPlugin : IPlugin
    {
        public static Dictionary<string, DockingPaneViewModel> DemoPlanVmsDic;

        public void Install(IPluginInfo pluginInfo)
        {
            DemoPlanVmsDic = new Dictionary<string, DockingPaneViewModel>();
            var dm = new DockingPaneViewModel(new DemoCreateViewModel())
            {
                Header = "Demo",
                IsDocument = true
            };
            DemoPlanVmsDic["demo"] = dm;
            M.DockingManager.InsertPane(dm);
        }
      
        public void Uninstall()
        {
        }
    }
}
```

运行程序，看下效果吧！

现在，你已经可以完成了`ViewModel`和`View`的创建、WPF基本控件的使用与显示。继续加油！