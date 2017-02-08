using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AxisMgntSample.Data;
using AxisMgntSample.Entities;
using WallE.Core;
using WallE.Core.Extend;
using WallE.Core.Models;

namespace AxisMgntSample
{
    public class AxisSamplePlugin : IPlugin
    {
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

        /// <summary>
        /// Sets the name of the dispaly table.
        /// </summary>
        private void SetDispalyTableName()
        {
            Tables.SetDisplayName(Axis.TableName, "轴线信息表");
            Tables.SetDisplayName(Level.TableName, "层信息表");
        }
    }
}
