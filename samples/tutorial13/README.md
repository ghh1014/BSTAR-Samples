# RadGridView绑定

之前，已经简单介绍了`RadGridView`列表控件的使用，下面我们来详细说明一下。

- `View`层

查看之前`RadGridView`控件绑定，前端示例代码。控件数据集合通过`ItemsSource="{Binding UserSource}"`绑定，其中`UserSource`为`ViewModel`层设置；选中`Item`的设置为：`SelectedItem="{Binding UserItem}"`，其中`UserItem`为当前鼠标选中的数据，`ViewModel`层设置；` <telerik:GridViewDataColumn Header="序号" DataMemberBinding="{Binding Number}" />`,`Number`为`User`类中的属性，其他列以此类推。

- `ViewModel`层

查看之前`RadGridView`控件绑定，后端代码。