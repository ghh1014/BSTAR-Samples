using System.IO;
using WallE.Assist;
using WallE.Core;

namespace CreatePluginDemo
{
    public class GlobalConfig: SettingBase
    {
        private static readonly GlobalConfig _instance;

        public static GlobalConfig Ins
        {
            get { return _instance; }
        }

        static GlobalConfig()
        {
            _instance = new GlobalConfig();
        }

        public bool AlwaysShow
        {
            get { return Get<bool>("AlwaysShow"); }
            set { Set("AlwaysShow", value); }
        }

        public string Name
        {
            get { return Get<string>("Name"); }
            set { Set("Name", value); }
        }

        private GlobalConfig() : base(Path.Combine(CoreUtil.AppSettingDirectory, "MyFirstPlugin_Setting.json"))//配置的信息将作为一个json文件储存起来
        {
            //设置插件第一次配置的初始值
            SetDefaut("AlwaysShow", true);
            SetDefaut("Name", "您的名字是？");
        }
    }
}