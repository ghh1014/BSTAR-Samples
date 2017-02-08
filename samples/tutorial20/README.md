### 发布/使用服务

#### 1、服务的注册与取消注册

一般在安装插件时（Install函数中）注册服务，卸载插件时（Uninstall函数中）取消注册服务。使用`Service`类提供的方法进行服务的注册操作，根据不同的服务类型调用不同的方法（New，NewFunc，NewAsync，NewFuncAsync），相关代码如下所示：

```C#
public class NoticePlugin : IPlugin
{
    /// <summary>
    /// 本插件注册的服务集合
    /// </summary>
    private List<Service> _services;

    public void Install(IPluginInfo pluginInfo)
    {
        SetDispalyTableName();
        AxisPluginService.Ins.Init();
        //初始化数据工厂
        AxisData.Factory.Init();
        //注册服务
        //注册服务
        _services = new List<Service>
        {
            Service.NewFuncAsync("__AxisService__:GetAllLevelName", GetAllLevelNameAsync)
        };
        _services.ForEach(ServiceHub.Register);
    }

    public void Uninstall()
    {
        AxisPluginService.Ins.Dispose();
        //释放数据工厂
        AxisData.Factory.Release();
        //取消服务注册
        _services.ForEach(t => ServiceHub.Unregister(t.Id));
        _services.Clear();
    }

    /// <summary>
    /// 获取标高数据（服务方法）
    /// </summary>
    private async Task<Result> GetAllLevelNameAsync(object param)
    {
        var projectItemKey = param as string;
        if (string.IsNullOrEmpty(projectItemKey))
            return new Result(new Exception("调用验证层级的服务时传入的参数有误！"));
        var axisData = AxisData.Factory.Get(projectItemKey);
        //获取标高信息
        LoggerHelper.Info("加载标高数据...");
        var levelRecords = await axisData.GetAllLevelAsync();
        if (levelRecords.Count == 0)
            return new Result(new Exception("未能加载标高数据，需要导入轴网信息！"));
        var levelNames = levelRecords.Select(record => (string)record[Level.ColName]).ToList();
        return new Result(levelNames);
    }
}
```

#### 2、服务类型、参数及使用方式

2.1、服务类型：无参数，无返回值；一个`object`类型的参数，无返回值；无参数，返回`Result`；一个`object`类型的参数，返回Result；无参数，返回`Task`（异步服务）；一个`object`类型的参数，返回`Task`（异步服务）；无参数，返回`Task<Result>`（异步服务）；一个`object`类型的参数，返回`Task<Result>`（异步服务）

2.2、参数传递方式，当传递的参数个数比较少时使用Tuple进行传递，可根据实际情况使用的方式：使用`string（Json）`方式传递参数；使用匿名对象传递参数，解析参数的时候使用`param.GetPropertyValue("属性名", "默认值")`；使用字典（`Dictionary<string,object>`）的方式传递参数；使用动态对象（`dynamic）`传递参数。

2.3、使用服务

```c#
//调用服务获取标高名称
var r = await ServiceHub.InvokeAsync("__AxisService__:GetAllLevelName", "FFD12133-3C36-4ADE-A2AF-FF728494434F");
if (r == null || !r.Any())
{
    this.ShowMessage("无法调用验证层级的服务，需加载轴线管理模块！", "层级验证失败", MessageBoxButton.OK);
    return null;
}
var result = r[0];
if (result.ResultType != ResultType.Ok)
{
    this.ShowMessage(result.Message, "层级验证失败", MessageBoxButton.OK);
    return null;
}
var levelNames = result.Data as List<string>;
if (levelNames != null)
    //此处省略其它代码......
```

**PS：**看完服务的使用，你可能会说我不清楚其它插件提供的服务需要传入哪些参数，这时你可以打开对应插件提供的说明文档。当然如果你创建的插件提供了相关服务，那就应该提供服务的说明文档，详细说明该插件提供哪些服务，参数类型等
