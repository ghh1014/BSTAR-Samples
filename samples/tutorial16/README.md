# View层与ViewModel层之间消息通信

`View`层与`ViewModel`层可以相互发消息，一端发一端接收，下面给出详细介绍。

- `string`消息的发送与接收

`View`层消息发送，参考代码：

```c#
Messager.Send(this, new Message<string>("ViewToViewModel", "我是通信消息2"));
```

消息接收，`ViewModel`层接收消息，参考代码：

```c#
/// <summary>
/// string消息接收
/// </summary>
/// <param name="message">string</param>
public override void Handle(Message<string> message)
{
    base.Handle(message);
    if (message.Name == "ViewModelToView")
    {
        //进行一些操作
        Btn.Content = $"{message.Value}";
    }
}
```

> 其中`Btn`为前端某按钮的`Name`

如果是`View`层接收消息的方法，不需要添加override。

- `Object`消息的发送与接收

有时发送的消息可能是一个`Object`对象，所以，`string`消息不适用了。此时，需要类引用`IHandle<Object>`。

`ViewModel`层消息发送，参考代码：

```c#
var user = new User { Number = "4", Name = "小黑", Remark = "-" };
Messager.Send(this, new Message<User>("ViewToViewModel", user))
```

`View`层接收消息，参考代码：

```c#
/// <summary>
/// 实体数据消息接收
/// </summary>
/// <param name="message"></param>
public void Handle(Message<User> message)
{
    if (message.Name == "ViewToViewModel")
    {
        //进行一些操作
        UserSource.Add(message.Value);
    }
}
```

> 其中`UserSource`为上面例子中`RadGridView`的数据源。

这样，实现了两个层消息的发送。