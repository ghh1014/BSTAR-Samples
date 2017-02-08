## 命令的分类

- 命令主要分为两种，一种为插件内的命令，这里叫做内部命令；一种提供全局使用的命令，这里叫做全局命令。


- 内部命令：内部命令只在插件内部使用，其他的插件无法使用，主要处理插件中按钮的事件的绑定，详情见[事件绑定](https://github.com/bstar5/BSTAR-Samples/tree/master/samples/tutorial7)
- 全局命令：全局命令在插件内部定义，在其他插件中也可以获取该命令，主要用于提供命令给其他插件或其他不能通过内部命令完成的地方使用。

## 命令的定义及使用

所有的命令都继承自`ICommand`；其具体的实现主要有两种`RelayCommand`，`AsyncCommand`：

- `RelayCommand`：

  ```C#
  ICommand cmd = new RelayCommand(OnAction, CanAction);
  //ICommand cmd = new RelayCommand(OnAction);
  private void OnAction(){}
  private bool CanAction(){
    return true;
  }
  ```

  **参数说明**：`OnAction` 为触发该命令后执行的方法，该方法可以为无参数方法或者为包含一个`object`参数的方法 ；`CanAction`为判断该命令是否可用的方法，该方法的返回值为`bool`类型；两个方法都不能是返回Task的方法。

  由于`CanAction`有返回值，所以不能是异步方法。

- `AsyncCommand`: 该命令是对`RelayCommand`的一个封装，使得`CanAction`能够返回`Task<bool>`类型，其包含`RelayCommand`的属性，名为`Command`，通过`Command`就可以使用其委托的方法；

  ```C#
  AsyncCommand cmd = new AsyncCommand(OnAction, CanActionAsync);
  private void OnAction(){}
  private async Task<bool> CanActionAsync(){
    return true;
  }
  ```

**提示**：由于在获取权限的时候需要使用异步的方法，所以如果判断一个命令的是否可用，需要根据权限来判断 则需要使用`AsyncCommand` 来实现。

## 注册/移除全局命令

​	系统通过`ICommandManager` 来管理全局的命令，位于`Core.M`中，通过`Register`方法可以注册命令，通过`Unregister`卸载命令。

- 注册命令：

  ```c#
  M.CommandManager.Register(new CommandInfo("CommandName", cmd) { DisplayName = "NameString" });
  ```

  **参数说明**：`Register`需要提供`CommandInfo`类型的参数； `CommandInfo`的构造函数中，`CommandName`表明命令的唯一标识名，`cmd`是传入的`ICommand `命令；

  **注意**：`AsyncCommand` 并不是`ICommand` 的实现，所以不能传入`CommandInfo`的构造函数中，需要使用其`Command`属性。如下：

  ```C#
  AsyncCommand cmd = new AsyncCommand(OnAction, CanActionAsync);
  M.CommandManager.Register(new CommandInfo("CommandName", cmd.Command) { DisplayName = "NameString" });
  ```

- 移除命令：

  ```C#
  M.CommandManager.Unregister("CommandName");
  ```

  **参数说明**：`CommandName`为注册命令时使用的唯一标识名。