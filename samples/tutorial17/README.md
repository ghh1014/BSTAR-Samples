### 创建插件需要的数据表

​	如果插件需要在数据库中创建相应的表来管理数据，出于数据安全的考量，此时你可能无法接触到项目对应的数据库，即使可以管理数据库，但是要为每个正在使用的项目添加你需要的数据库表也是一件非常头疼的事情，为此，BIM-STAR支持插件创建其相应的数据表：在打开某个项目加载对应插件的时候，首先检查数据库中是否执行了此插件的数据表定义文件，如果没有（或不是最新版本）则会执行所有新版本的数据表定义文件，这样就把插件需要的数据表添加到对应项目的数据库中。

#### 1、Sql文件的创建及插件配置

1.1、在SQL SERVER数据库中创建相关表并导出为sql文件（v1.sql），设计数据库表的时候需要充分考虑，从而避免后面对数据库表进行改动，即使需要改动的话需要注意不能影响正在使用的数据（特别是上线的项目）。插件每次更新数据库表时都需要更新相应的版本：如果新版的插件修改了某个表，则需要手动写表改动的sql语句并形成新的sql文件v(x+1).sql；对于插件添加新表的话，先在SQL SERVER数据库中创建新表并导出为sql文件v(x+1).sql。

**PS：**通常在设计数据库表的时候会加入一个`ExtentJsonInfo`字段，这个字段的数据库类型为字符串，实际内容为`json`格式，这样后面如果要修改已经上线的数据表，就可以不用更改数据库了，只需要更改该字段对应的类

1.2、所有版本的sql文件都存放在插件的`Sqls`目录下，需要设置sql文件属性使其拷贝到执行目录下。

1.3、插件配置文件（`config.plugin`）中需要添加`SqlFiles`节点，在`SqlFiles`节点下可添加多个`SqlFile`节点，其中`SqlFile`节点需要设置sql文件的版本及在插件中的路径

```xml
<?xml version="1.0" encoding="utf-8" ?>
<Plugin Key="5B07034C-C0DF-4F0E-A225-B0683D5BA9B6" 
        FileName="AxisMgntSample.dll"
        Name="轴线管理(示例)"
        Description="使用该插件导入轴线信息，为4D自动关联提供支持"
        Developer="深圳筑星科技有限公司"
        LoadTime="LoadWhenProjectOpened"
        LoadOrder="10"
        Version="1.0.0.0"
        WebUrl="http://www.bstar5.com"
        Icon="Assets/logo.png"
        IsEnable="true">
  <SqlFiles>
    <SqlFile Name="sqls/v1.sql" Version="1"/>
    <SqlFile Name="sqls/v2.sql" Version="2"/>
  </SqlFiles>
</Plugin>
```

#### 2、定义与表对应的实体类及其使用

2.1、尽管框架中统一采用`IDynamicRecord`来表示一条记录，但还是建议定义实体类，通常作为临时变量使用，有如下好处：智能感知提示，方便传参，避免通过写字符串来访问数据，下面是轴线实体类

```C#
public class Axis : EntityBase
{
    //数据库表名
    public const string TableName = "Pl_Axis_AxisInfo";
    //数据库中Id字段
    public const string ColId = "Id";
    public const string ColName = "Name";
    public const string ColX1 = "X1";
    public const string ColY1 = "Y1";
    public const string ColZ1 = "Z1";
    public const string ColX2 = "X2";
    public const string ColY2 = "Y2";
    public const string ColZ2 = "Z2";
    public const string ColType = "Type";

    #region 属性字段
    public int Id { get; set; }

    public string Name { get; set; }

    public double X1 { get; set; }

    public double Y1 { get; set; }

    public double Z1 { get; set; }

    public double X2 { get; set; }

    public double Y2 { get; set; }

    public double Z2 { get; set; }

    public int Type { get; set; }
    #endregion
      
    public override string MapPropertyName(string propertyName)
    {
        return propertyName;
    }
}
```

2.2、实体类通常作为临时变量使用，`IDynamicRecord`可通过框架提供的`As<T>()`转为实体类，实体类可通过`ToDictionary()`转为数据操作时需要传入的参数

```c#
//修改维护计划，首先把IDynamicRecord转为Plan实体类
var plan = SelectPlan.PlanRecord.As<Plan>();
//修改实体类的信息
plan.Name = planVm.Name;
plan.CodeId = planVm.CodeId;
...
//更新IDynamicRecord的数据
SelectPlan.PlanRecord.SetValue(plan.ToDictionary());
```

### 
