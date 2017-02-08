using AxisMgntSample.Entities;
using WallE.Core;

namespace AxisMgntSample
{
    public class AxisSamplePlugin : IPlugin
    {
        public void Install(IPluginInfo pluginInfo)
        {
            SetDispalyTableName();
            AxisPluginService.Ins.Init();
            //初始化数据工厂......
            //注册服务......
        }

        public void Uninstall()
        {
            AxisPluginService.Ins.Dispose();
            //释放数据工厂......
            //取消服务注册......
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
