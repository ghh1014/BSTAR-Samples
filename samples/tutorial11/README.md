# 控件主题使用

- 控件使用主题颜色

控件使用主题的颜色，包括背景色或者前景色等，下面是背景色的示例：

```xml
<Button Background="{Binding Source={x:Static core:M.ThemeManager},Path=AccentBrush}"/>
```

其中，`Path`为`SolidColorBrush`的不同属性，可根据实际使用情况，选择不同的`Brush`可供选择， 下面有部分示例：

```c#
public interface IThemeManager
{
  IReadOnlyList<ThemeInfo> Themes { get; }
  
  ThemeInfo CurrentTheme { get; }

  SolidColorBrush AccentMainBrush { get; set; }

  SolidColorBrush AccentDarkBrush { get; set; }

  SolidColorBrush SelectedBrush { get; set; }

  SolidColorBrush ValidationBrush { get; set; }

  SolidColorBrush AccentBrush { get; set; }

  SolidColorBrush MainBrush { get; set; }

  SolidColorBrush MakerBrush { get; set; }

  SolidColorBrush StrongBrush { get; set; }

  SolidColorBrush PrimaryBrush { get; set; }

  SolidColorBrush AlternativeBrush { get; set; }

  SolidColorBrush MouseOverBrush { get; set; }
}
```

- 控件主题自适应

如果，有比较复杂的样式，需要引入外部资源文件时，需要`xaml`后端添加方法。参考方法，如下：（有待完善）

```c#
protected override void OnRefreshTheme()
{
    base.OnRefreshTheme();
    Resources = new ResourceDictionary
    {
      	//路径
        Source = new Uri("/WbsExplorer;component/Views/WbsMainViewRes.xaml", UriKind.RelativeOrAbsolute)
    };
}
```