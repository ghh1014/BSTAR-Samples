# 图片素材的绑定

- 添加图片

控件中添加`Image`，赋值`Source`。示例，如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="控件背景图片：" VerticalAlignment="Center" HorizontalAlignment="Right" />
    <Button Grid.Column="1">
        <Image Source="{core:PluginResource ImagePath=Assets/logo.png}" />
    </Button>
</Grid>
```

其中`Assets/logo.png`为图片路径。

- `ViewBox`添加Path

在控件中添加`ViewBox`，示例如下：

```xml
<Grid Margin="10">
    <Grid.ColumnDefinitions>
        <ColumnDefinition Width="150" />
        <ColumnDefinition Width="Auto" />
    </Grid.ColumnDefinitions>
    <TextBlock Text="控件背景ViewBox：" VerticalAlignment="Center" HorizontalAlignment="Right" />
    <Button Grid.Column="1">
        <Viewbox>
            <Path Data="M299.152955025434,0L496.426178902388,0 268.890046089888,280.247772216797 198.190201729536,367.407897949219 199.87503144145,369.495361328125 271.502137154341,457.706451416016 498.999970406294,738 301.717621773481,738 101.234124153852,491.008697509766 99.5304498374462,488.936553955078 98.6409082114697,490.003173828125 0,368.488952636719 98.6409082114697,246.945526123047z"
              Stretch="Uniform" Fill="{Binding Source={x:Static core:M.ThemeManager},Path=AccentBrush}" Width="26" Height="26" />
        </Viewbox>
    </Button>
</Grid>
```

其中，`Path`通过`Metro Studio`获得的，不同图有不同的`Data`，以上例子仅做参考。具体详情请查看`Metro Studio`的使用

以上，两种方式，可以给控件添加图片啦。