### 通过数据工厂管理数据

#### 1、数据管理类的定义

1.1、定义一个类继承`ProjDataBase`（该类实现了`IProjData`接口），并定义私有构造函数

1.2、在类中定义一个工厂类，继承自`ProjDataFactory<T>`

1.3、在类中定义工厂类的静态实例

1.4、在类中提供一系列插件中用到的数据接口

```C#
/// <summary>
/// 轴线数据管理类（可以管理多个数据表，示例只列出轴线数据表作为参考）
/// </summary>
internal class AxisData : ProjDataBase
    {
        #region 工厂定义（需要传入子项目信息）
        public class AxisDataFactory : ProjDataFactory<AxisData>
        {
            protected override AxisData New(ProjectItem projectItem)
            {
                return new AxisData(projectItem);
            }
        }

        public static AxisDataFactory Factory = new AxisDataFactory();

        private AxisData(ProjectItem projItem)
            : base(projItem)
        {
            //初始化轴线对应的RecordHub
            _axisHub = InitRecordHub(Axis.TableName);
        }
        #endregion

        //轴线对应的RecordHub
        private readonly IRecordHub _axisHub;
        //数据加载状态
        private LoadState _loadState;
        //轴线数据更改事件（数据有变化时会触发该事件）
        public event EventHandler<StatusChangedEventArgs> AxisRecordStatusChanged;

        /// <summary>
        /// 数据加载方法（有些数据不需要全部加载，因此可以省略该方法）
        /// </summary>
        protected async Task<Result> LoadDataAsync(INotifier notifier)
        {
            if (_loadState != LoadState.Unload)
                return Result.Ok;
            _loadState = LoadState.Loading;
            try
            {
                //加载所有轴线数据
                var r = await _axisHub.LoadAllAsync(notifier);
                if (r.ResultType != ResultType.Ok)
                    throw r.Error;
                //设置加载状态
                _loadState = LoadState.Loaded;
                return Result.Ok;
            }
            catch (Exception e)
            {
                _axisHub.Dispose();
                _loadState = LoadState.Unload;
                return new Result(e);
            }
        }

        /// <summary>
        /// 子项目加载后会执行的方法
        /// </summary>
        protected override Task<Result> OnLoadingProjectItemAsync(INotifier notifier)
        {
            //触发轴线更改事件，订阅了该事件的地方可以处理相关事情
            if (AxisRecordStatusChanged != null)
                AxisRecordStatusChanged(this, null);
            return base.OnLoadingProjectItemAsync(notifier);
        }

        /// <summary>
        /// 关闭子项目时需要执行的方法
        /// </summary>
        protected override Task<Result> OnUnloadingProjectItemAsync(INotifier notifier)
        {
            //关闭子项目对应面板
            AxisPluginService.Ins.CloseDockingPane(ProjectItem.Key);
            return base.OnUnloadingProjectItemAsync(notifier); 
        }

        /// <summary>
        /// RecordHub状态更改时会执行的方法，此处可以触发AxisRecordStatusChanged事件
        /// </summary>
        protected override void OnRecordHubStatusChanged(object sender, StatusChangedEventArgs e)
        {
            base.OnRecordHubStatusChanged(sender, e);
            var hub = sender as IRecordHub;
            if (hub == _axisHub)
            {
                if (AxisRecordStatusChanged != null)
                    AxisRecordStatusChanged(this, e);
            }
        }

        /// <summary>
        /// 数据管理类的属性更改时会执行的方法
        /// </summary>
        protected override void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            base.OnPropertyChanged(e);
            //判断数据是否更改，然后决定面板是否显示 *
            if (e.PropertyName == "IsDirty")
                AxisPluginService.Ins.UpdateHeaderStatus(ProjectItem.Key, IsDirty);
        }

        #region 轴线数据的操作方法

        /// <summary>
        /// 加载所有轴线信息
        /// </summary>
        internal async Task<ICollection<IDynamicRecord>> GetAllAxisAsync()
        {
            var r = await LoadDataAsync(null);
            if (r.ResultType != ResultType.Ok)
                LoggerHelper.Error("加载轴线数据失败！", r.Error);
            return _axisHub.Records.ToList();
        }

        /// <summary>
        /// 删除所有轴线信息
        /// </summary>
        internal async Task<Result> DeleteAllAxisAsync()
        {
            return await _axisHub.DeleteManyAsync(_axisHub.Records);
        }

        /// <summary>
        /// 新建轴线信息
        /// </summary>
        /// <returns></returns>
        internal async Task<List<IDynamicRecord>> CreateAxisAsync(List<Axis> axisList)
        {
            return await _axisHub.NewRecordsAsync(axisList.Select(t => t.ToDictionary()).ToList(), true);
        }

        /// <summary>
        /// 修改轴线记录
        /// </summary>
        /// <param name="record">原始记录</param>
        /// <param name="modifyRecord">修改后的信息字典</param>
        /// <returns></returns>
        internal async Task<Result> ModifyAxisAsync(IDynamicRecord record, Dictionary<string, object> modifyRecord)
        {
            if (record == null)
                return Result.Ok;
            return await _axisHub.ModifyRecordAsync(record, modifyRecord);
        }

        #endregion
    }
```

#### 2、数据管理类的初始化及使用

2.1、插件实现`IPlugin`接口的类中需要对数据工厂进行初始化及释放

```c#
public void Install(IPluginInfo pluginInfo)
{
    //数据工厂初始化,xxxData.Factory.Init()
    AxisData.Factory.Init();
    ......//此处省略其它代码
}

public void Uninstall()
{
    //数据工厂释放,xxxData.Factory.Release()
    AxisData.Factory.Release();
    ......//此处省略其它代码
}
```

2.2、采用`xxxData.Factory.Get(string proItemKey)`来获取该数据管理类，获取到管理类后就可以调用定义的数据接口来管理数据

```C#
//获取轴线数据管理类
var axisData = AxisData.Factory.Get(projectItem.Key);
```

#### 3、多用户协作，数据锁定及同步显示

​	多个用户同时加载了同一个子项目，当用户A修改构件名称或导入模型等操作，并进行保存，用户B就能实时看到修改后的构件名称或图形平台自动加载并显示新导入的模型；另外一个情形是用户A修改了构件的名称，但是并没有保存数据，这是用户B就不能修改同一构件的名称了。

3.1、数据记录同步显示，这里需要监听数据管理类中的数据更改事件，当某个表有新增数据或者删除数据时需要更新界面绑定的集合

```c#
/// <summary>
/// 数据加载方法
/// </summary>
protected override async void OnViewLoaded(bool isFirstLoaded)
{
    base.OnViewLoaded(isFirstLoaded);
    IsBusy = true;
    _axisInfos.Clear();
    _spaceLevels.Clear();
    //加载所有数据
    var records = await _axisData.GetAllAxisAsync();
    foreach (var record in records)
    {
        //把IDynamicRecord转成界面中绑定的model
        var axisInfo = new AxisInfo(record);
        _axisInfos.Add(axisInfo);
    }
    var spaceLevels = new List<LevelInfo>();
    var recordList = await _axisData.GetAllLevelAsync();
    foreach (var record in recordList)
    {
        var levelInfo = new LevelInfo(record) { AxisData = _axisData };
        spaceLevels.Add(levelInfo);
    }
    spaceLevels = spaceLevels.OrderByDescending(t => t.StartElevation).ToList();
    _spaceLevels.AddRange(spaceLevels);
    //加载万所有数据后监听数据管理类的状态更改事件
    WeakEventManager<AxisData, StatusChangedEventArgs>.AddHandler(_axisData, "AxisRecordStatusChanged",
        OnAxisRecordStatusChanged);
    WeakEventManager<AxisData, StatusChangedEventArgs>.AddHandler(_axisData, "LevelRecordStatusChanged",
        OnLevelRecordStatusChanged);
    IsBusy = false;
}

/// <summary>
/// 数据同步（处理更改事件）
/// </summary>
/// <param name="sender">The sender.</param>
/// <param name="e">The <see cref="StatusChangedEventArgs"/> instance containing the event data.</param>
private void OnLevelRecordStatusChanged(object sender, StatusChangedEventArgs e)
{
    //处理新增的记录
    if (e.Action == StatusChangedAction.Added)
    {
        var records = e.Records;
        var existIds = _spaceLevels.Select(t => t.Id).ToList();
        var hasDataChange = false;
        for (var i = 0; i < records.Count; i++)
        {
            var record = records[i];
            if (!existIds.Contains((int)record[Level.ColId]))
            {
                var levelInfo = new LevelInfo(record) { AxisData = _axisData };
                _spaceLevels.Add(levelInfo);
                hasDataChange = true;
            }
        }
        if (hasDataChange)
        {
            var spaceLevels = _spaceLevels.OrderByDescending(t => t.StartElevation).ToList();
            _spaceLevels.Clear();
            _spaceLevels.AddRange(spaceLevels);
        }
    }
    //处理删除的记录
    else if (e.Action == StatusChangedAction.Deleted)
    {
        var delItems = _spaceLevels.Where(t => e.Records.Contains(t.LevelRecord)).ToList();
        delItems.ForEach(t => _spaceLevels.Remove(t));
    }
    //处理清除
    else if (e.Action == StatusChangedAction.Reset)
        SpaceLevels.Clear();
}
```

3.2、单条数据记录的同步显示，这个需要在界面绑定的Model中进行处理，当`IDynamicRecord`的属性更改时需要处理界面绑定的Model，这样界面才会进行刷新

```C#
/// <summary>
/// 轴线界面绑定Model
/// </summary>
public class AxisInfo : NotifyObject
{
    /// <summary>
    /// 构造方法，传入IDynamicRecord
    /// </summary>
    public AxisInfo(IDynamicRecord record)
    {
        AxisRecord = record;
        InitAxisInfo();
        Key = Guid.NewGuid().ToString();
        AxisRecord.PropertyChanged -= AxisRecord_PropertyChanged;
        //监听IDynamicRecord属性更改事件
        AxisRecord.PropertyChanged += AxisRecord_PropertyChanged;
    }

    public AxisInfo() { }

    /// <summary>
    /// IDynamicRecord属性更改需要执行的方法
    /// </summary>
    private void AxisRecord_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        InitAxisInfo();
    }

    /// <summary>
    /// 属性赋值操作
    /// </summary>
    private void InitAxisInfo()
    {
        Id = (int)AxisRecord[Axis.ColId];
        Name = (string)AxisRecord[Axis.ColName];
        Type = (AxisType)(int)AxisRecord[Axis.ColType];
        X1 = (double)AxisRecord[Axis.ColX1];
        Y1 = (double)AxisRecord[Axis.ColY1];
        Z1 = (double)AxisRecord[Axis.ColZ1];
        X2 = (double)AxisRecord[Axis.ColX2];
        Y2 = (double)AxisRecord[Axis.ColY2];
        Z2 = (double)AxisRecord[Axis.ColZ2];
    }

    public IDynamicRecord AxisRecord { get; private set; }

    private int _id;
    public int Id
    {
        get { return _id; }
        set { Set("Id", ref _id, value); }
    }

    private string _name;
    public string Name
    {
        get { return _name; }
        set { Set("Name", ref _name, value); }
    }
    ......//此处省略其他代码
}
```

3.3、数据的锁定及放弃数据更改。进入某个功能进行添加、修改或删除等操作后，如果需要放弃本次的更改该怎么办呢？框架中提供了`ScopeContext`来处理这个事情，进入功能操作之前需要针对一个或多个数据表创建`ScopeContext`，然后所有的操作都由这个创建`ScopeContext`进行管理，最后需要放弃本次更改时只需要调用`ScopeContext.CancelChangesAsync()`方法（还提供了撤销指定数据记录的方法）。这里在修改某个条数据的时候需要对其进行锁定，这样其它用户就不能对其进行修改，锁定的方式为`xxxRecordHub.CheckOutAsync(Records)`

```c#
private readonly ScopeContext _scope;

/// <summary>
/// 构造方法
/// </summary>
public RenderStyleViewModel(IProjectItemNode proItemNode)
{
    _proItemNode = proItemNode;
    //获取数据管理类
    var data = ProgressData.Factory.Get(_proItemNode);
    //创建ScopeContext，并传入数据管理类中的表
    _scope = new ScopeContext(data.RenderStyleHub);
}

private async void OnEditRenderStyle()
{
    //获取数据管理类
    var data = ProgressData.Factory.Get(_proItemNode);
    //锁定需要更改的记录
    var result = await data.RenderStyleHub.CheckOutAsync(SelectedRs.RsRecord);
    if (result.ResultType != ResultType.Ok)
    {
        var msg = $"无法修改渲染样式'{SelectedRs.Name}'\r\n{result.Message}";
        this.ShowMessage(msg);
        return;
    }
    //进入修改功能，并更改数据
    var rsVm = new EditRenderStyleViewModel(SelectedRs);
    var vm = new DialogViewModel(rsVm)
    {
        Width = 480,
        OkContent = "保存",
        SizeToContent = SizeToContent.Height,
        Title = $"修改'{SelectedRs.Name}'",
    };
    var r = M.DialogManager.ShowDialog(vm);
    if (r != CloseResult.Ok)
    {
        //取消更改时则需要撤销更改,这时也会解除数据的锁定
        await _scope.CancelChangesAsync(SelectedRs.RsRecord);
        return;
    }
    ......//此处省略其他代码
}
```

