系统中的右键菜单采用的`telerik`的`RadContextMenu` 控件，关于此控件的用法见[telerik控件的使用说明](https://github.com/bstar5/BSTAR-Samples/tree/master/samples/tutorial10)

## 创建右键菜单

- 创建`MenuHub`对象

  ```C#
   MenuHub _menuHub = new MenuHub(menuNames);
  // MenuHub _menuHub = new MenuHub(fasel,menuNames);
  ```

  **参数解释**：`MenuHub`有两个重载的构造函数：

  - 带一个参数的构造函数：该参数是类型为`string[]`的可变参数，包含一组菜单的唯一标识名。

  - 带两个参数的构造函数：第一个参数用于控制该菜单是否允许其他的插件向在其中扩展命令。`FALSE`为不允许，

    ​                                           第二个参数为`string`类型的可变参数，表示菜单的唯一标识名。

## 右键菜单中添加菜单项

- 插件内部添加，直接使用`MenuHub` 对象的`Register`方法，如下：

  ```C#
  MenuItemInfo mItemInfo= new MenuItemInfo("DisplayName", "menuName") { GroupName = "GroupName" };
  //MenuItemInfo m18= new MenuItemInfo("DisplayName", cmd) { GroupName = "GroupName" };
  _menuHub.Register(menuName, mItemInfo, 0);
  ```

  **说明**：`MenuItemInfo`对象包含两个构造函，其中第一个参数为显示的名称，第二的参数可以为在全局命令中定义的命令的唯一标识名（见[全局命令的使用](https://github.com/bstar5/BSTAR-Samples/tree/master/samples/tutorial07)），或者直接使用`ICommand`对象（见[命令的使用](https://github.com/bstar5/BSTAR-Samples/tree/master/samples/tutorial07)）。 `GroupName` 为一组菜单项的组名的标识名，不同组之间有分割线分割。

  ​	    `Register`方法包含三个参数，`MenuName`为已知的菜单名，第二个参数为`MenuItemInfo`对象，第三个为菜单的位置。

- 外部插件添加，需要使用`IMenuManager`中的`Register`方法 可以通过`M.MenuManager`获得该对象，如下：

  ```c#
  MenuItemInfo mItemInfo= new MenuItemInfo("DisplayName", "menuName") { GroupName = "GroupName" };
  M.MenuManager.Register(menuName, mItemInfo);
  ```

  **说明**：`MenuName`为目标菜单的标识名。

## 卸载菜单

菜单的卸载主要为外部插件需要使用，可以通过`IMenuManager`中的`Unregister`方法 该对象可以从`M.MenuManager`得到。如下：

```c#
MenuItemInfo mItemInfo= new MenuItemInfo("DisplayName", "menuName") { GroupName = "GroupName" }; 
M.MenuManager.Unregister("menuName", mItemInfo);
```

**说明**： 上述实例会卸载`menuName`菜单中的显示名称为`DisplayName`的菜单项。