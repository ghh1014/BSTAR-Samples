# 框架基本控件

下面介绍，框架中有基本控件的使用。首先，添加引用`WallE.Assist.dll`，所在目录为：`libs\WallE`。。

- MetroProgressBar

`MetroProgressBar`为进度条，多用在大量数据加载的时候，呈现出一个正在加载数据的状态。像`RadGridView`控件通常需要添加进度条。下面详细说明如何，添加进度条。

首先，在`View`层，前端代码中添加`Resources`中添加`converters:BoolToVisibilityCVT`，代码如下：

```xml
<core:ViewBase.Resources>
    <converters:BoolToVisibilityCvt x:Key="BoolToVisibilityCvt" />
</core:ViewBase.Resources>
```

然后，在`View`层，需要显示进度条的地方添加`MetroProgressBar`控件，代码如下：

```xml
<controls:MetroProgressBar VerticalAlignment="Bottom" VerticalContentAlignment="Bottom"
                                       IsIndeterminate="True" MaxWidth="300" Margin="10,0,0,-2"
                                       Visibility="{Binding IsBusy, Converter={StaticResource BoolToVisibilityCvt}}" />
```

接在，在`ViewModel`层，添加`IsBusy`属性，如下：

```c#
private bool _isBusy;
/// <summary>
/// 获取或设置IsBusy属性
/// </summary>
public bool IsBusy
{
    get { return _isBusy; }
    set { Set("IsBusy", ref _isBusy, value); }
}
```

最后，在数据加载的地方，设置`IsBusy`的值`true`or`false`来控制进度条的显示与隐藏。示例如下：

```c#
IsBusy = true;
UserSource = new ObservableCollection<User>
{
    new User{Number = "1",Name="小茗",Remark="-"},
    new User{Number = "2",Name="冷冷",Remark="-"},
    new User{Number = "3",Name="暖暖",Remark="-"}
};
IsBusy = false;
```

- SearchBox

> 对于很多数据，通常会用到搜索功能。下面，介绍如何使用`SearchBox`控件。

首先，在`View`层，添加`PromptBox`控件，代码如下：

```xml
<controls:SearchBox Grid.Row="1"  Margin="10,10,10,0" MinWidth="200" SearchFunc="{Binding Search}" />
```

然后，在`ViewModel`层，添加如下代码，代码如下：

```c#
#region SearchBox
  
private List<User> _bakUserSource;

public Func<string, Task> Search { get; private set; }

private async Task OnSearchAsync(string searchStr)
{
    if (string.IsNullOrEmpty(searchStr))
    {
        UserSource = new ObservableCollection<User>();
        UserSource.AddRange(_bakUserSource);
    }
    var searchItems = await Task.Run(() =>
    {
        var items = new ObservableCollection<User>();
        if (searchStr == null) return items;
        searchStr = searchStr.ToLower().Trim();
        foreach (var item in _bakUserSource)
        {
            if (ContainsString(item.Number + "", searchStr)
                || ContainsString(item.Name, searchStr)
                || ContainsString(item.Remark, searchStr)
                )
            {
                items.Add(item);
            }
        }
        return items;
    });
    UserSource = new ObservableCollection<User>(searchItems);
}

private static bool ContainsString(string str, string searchStr)
{
    return str != null && str.Contains(searchStr, CompareOptions.IgnoreCase);
}

#endregion SearchBox
```

> 其中`_bakUserSource`是数据集的一个备份，供搜索使用。

之后在`ViewModel`层类构造中，添加如下代码：

```c#
Search = OnSearchAsync;
```

到此，实现了`SearchBox`控件的显示、绑定及功能实现。

- PromptBox

首先，在`View`层，添加`PromptBox`控件，代码如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="PromptBox控件：" VerticalAlignment="Center" HorizontalAlignment="Right" />
    <controls:PromptBox Grid.Column="1" Placeholder="未填写" Text="{Binding Title,Mode=TwoWay}"
           TextWrapping="NoWrap" Foreground="{Binding MakerBrush, Source={x:Staticcore:M.ThemeManager}}" MinWidth="200" />
</Grid>
```

然后，在`ViewModel`层，添加`Title`属性，如下：

```c#
private string _title;

/// <summary>
/// 获取或设置Title属性
/// </summary>
public string Title
{
    get { return _title; }
    set { Set("Title", ref _title, value); }
}
```

这样，添加了`PromptBox`，而且实现了数据绑定。

- ValidationTip

当希望给控件添加验证时，需要用到`ValidationTip`控件。首先，在`View`层，添加`ValidationTip`控件，代码如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="PromptBox控件：" VerticalAlignment="Center" HorizontalAlignment="Right" />
    <controls:PromptBox Grid.Column="1" Placeholder="未填写" Name="Title" Text="{Binding Title,Mode=TwoWay,UpdateSourceTrigger=PropertyChanged,NotifyOnValidationError=True,ValidatesOnDataErrors=True}"
                        TextWrapping="NoWrap" Foreground="{Binding MakerBrush, Source={x:Static core:M.ThemeManager}}" MinWidth="200" />
    <controls:ValidationTip Grid.Column="2" Width="16" Height="16" ValidationElement="{Binding ElementName=Title}" Margin="4,0,0,0" VerticalAlignment="Center" />
</Grid>
```

`ValidationTip`控件指定`ValidationElement`要验证的控件`Name`。添加引用`System.ComponentModel.DataAnnotations.dll`，使用系统提供的验证。

然后，在`ViewModel`层，`Title`属性添加验证规则，这只是验证的一种，示例如下：

```c#
private string _title;

[Required(ErrorMessage = "Title不能为空！")]
public string Title
{
    get { return _title; }
    set { Set("Title", ref _title, value); }
}
```

这样，就可以对`Title`进行验证了。