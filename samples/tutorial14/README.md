# Button 命令绑定

按钮怎么相应命令呢，下面我们举个小例子，前端代码，如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="Button命令：" VerticalAlignment="Center" HorizontalAlignment="Right" />
    <Button Grid.Column="1" Content="点一下" Command="{Binding ClickMe}" />
</Grid>
```

其中，`ClickMe`为后端绑定的命令。然后，后端添加代码，如下：

```c#
public RelayCommand ClickMe { get; private set; }

private bool CanClickMe()
{
    return true;
}

private void OnClickMe()
{
	this.ShowMessage("点中我了");
}
```

并在`ViewModel`层的类构造函数中声明，如下：

```c#
ClickMe = new RelayCommand(OnClickMe, CanClickMe);
```

现在运行一下，点击按钮，查看一下效果吧。