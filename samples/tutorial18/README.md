### 通过`WebAPI`管理数据(主要关注2.2及其示例)

​	插件开发过程中，相关数据只能通过服务器端提供的`WebAPI`进行操作（或使用第三方的`WebAPI`）,`.Net`编程通常使用

`WebClient`或`HttpClient`与`WebAPI`接口进行交互，这两个类的详细用法请参阅相关书籍（或博客），下面对这两个类的使用进行简单介绍：

#### 1、通过`WebClient`与`WebAPI`接口进行交互

1.1、访问第三方提供的接口获取公网IP地址

```c#
/// <summary>
/// 获取公网IP地址
/// </summary>
/// <returns></returns>
public static string GetPubIpAddress()
{
    try
    {
        //创建WebClient
        var webClient = new WebClient();
        //WebAPI访问地址
        var baseUriString = "http://121.199.42.56:8089/Data/GetPubIpAddress" + "?" + "getTime=" + DateTime.Now;
        var serviceUri = new Uri(baseUriString, UriKind.Absolute);
      	//设置http请求头部信息
        webClient.Headers[HttpRequestHeader.ContentType] = "application/x-www-form-urlencoded";
        //获得请求返回结果
        var result = webClient.UploadString(serviceUri, "");
        if (!string.IsNullOrEmpty(result))
            return result;
        return "";
    }
    catch (Exception ex)
    {
        LoggerHelper.Debug("获取公网IP地址失败！", ex);
        return "";
    }
}
```

1.2、访问云服务器提供的接口获取产品更新日志

```C#
/// <summary>
/// 获取产品更新日志
/// </summary>
/// <returns></returns>
private void OnShowUpdateLog()
{
    try
    {
        //获取更新日志文件的url
        var uri = "http://112.74.212.41:8085/ProductLog/GetChangeLogFile?accessToken=bimstar&productName=BIM-STAR&productVersion=1.0.0.0";
        //获取更新日志文件名的url
        var getFileNameUri = "http://112.74.212.41:8085/ProductLog/GetLogFileName?accessToken=bimstar&productName=BIM-STAR&productVersion=1.0.0.0";
        //创建WebClient
        var webClient = new System.Net.WebClient();
        //设置请求头部信息
        webClient.Headers.Add("content-type", "application/xml; charset=utf-8");
        //请求更新日志的文件名
        var fileName = webClient.DownloadString(getFileNameUri);
        if (string.IsNullOrEmpty(fileName))
            return;
        //设置文件保存目录
        var fielPath = Path.Combine("E:\\", fileName);
        //创建WebClient
        webClient = new System.Net.WebClient();
        //下载更新日志文件
        webClient.DownloadFile(uri, fielPath);
        if (File.Exists(fielPath))
            Process.Start(fielPath);
    }
    catch (Exception ex)
    {
        //输出错误信息
        LoggerHelper.Error("从服务器上获取更新日志失败，请重新再试！", ex);
    }
}
```

#### 2、通过`HttpClient`与`WebAPI`接口进行交互

2.1、访问云服务器提供的接口获取视频教程信息

```c#
/// <summary>
/// 获取视频数据
/// </summary>
public async Task<List<VideoInfo>> GetVideoDatasAsync(string productName)
{
    const string commomErrorStr = "网络(或服务器)出错导致连接教程服务失败,请重新操作！";
    var videoItems = new List<VideoInfo>();
    try
    {
        //创建HttpClient并设置基地址
        using (var client = new HttpClient { BaseAddress = new Uri("http://112.74.212.41:8085/") })
        {
            //请求的API地址
            var url = "api/VideoTutorial/GetVideoTutorials?productName=BIM-STAR";
            //发起Get请求
            var response = await client.GetAsync(url);
            //判断请求是否成功
            if (!response.IsSuccessStatusCode)
                throw new Exception(commomErrorStr);
            //确保请求返回正确状态码
            response.EnsureSuccessStatusCode();
            //异步读取请求结果
            var resultInfo = await response.Content.ReadAsStringAsync();
            //将请求结果转为对应的类
            var resultModel = Json.ToObject<ResultModel>(resultInfo);
            return resultModel.VideoInfos;
        }
    }
    catch (HttpRequestException)
    {
        LoggerHelper.Error("无法连接到教程服务！", new Exception(commomErrorStr));
    }
    catch (Exception ex)
    {
        LoggerHelper.Error("无法连接到教程服务！", ex);
    }
    return videoItems;
}
```

2.2、访问项目服务器端提供的接口。首先参照服务器端相关接口的说明文档，在插件中定义好相应的类，这些类可以存放接口返回的结果或者存放需要提交的数据，框架中封装了通用方法访问`WebAPI`(`GET`及`POST`操作)，插件获取数据的时候不需要设置基地址（如 `http://112.74.212.41:8085/`），只需要传入相对地址（如`v1/AxisInfo/GetMajorInfos）`）。下面是获取轴网信息的示例方法。

```c#
/// <summary>
/// 通过接口获取标高信息
/// </summary>
/// <returns></returns>
public async Task<List<Level>> GetLevelInfosAsync(string projectItemKey)
{
    var curResult = await M.NetManager.GetAsync<LevelsResult>("v1/AxisInfo/GetLevelInfos", new
    {
        userId = M.EnvManager.CurrentUser.Id,
        projectItemKey = projectItemKey
    });
    if (!curResult.IsSuccess || curResult.ItemInfos.Count == 0)
        throw new Exception("获取轴线标高数据失败！");
    return curResult.ItemInfos;
}

/// <summary>
/// 通过接口获取轴网信息
/// </summary>
/// <returns></returns>
public async Task<List<Axis>> GetAxisInfosAsync(string projectItemKey)
{
    var curResult = await M.NetManager.GetAsync<AxisListResult>("v1/AxisInfo/GetAxisInfos", new
    {
        userId = M.EnvManager.CurrentUser.Id,
        projectItemKey = projectItemKey
    });
    if (!curResult.IsSuccess || curResult.ItemInfos.Count == 0)
        throw new Exception("获取轴线标高数据失败！");
    return curResult.ItemInfos;
}
```

### 