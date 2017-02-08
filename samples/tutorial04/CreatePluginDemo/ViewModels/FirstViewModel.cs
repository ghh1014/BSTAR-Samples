using WallE.Core;
namespace CreatePluginDemo.ViewModels
{
    public class FirstViewModel:ViewModelBase
    {

        private string _name;
        /// <summary>
        /// 获取或设置Name属性
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { Set("Name", ref _name, value); }
        }

        public FirstViewModel()
        {
            if (!string.IsNullOrEmpty(GlobalConfig.Ins.Name))
                Name = "您好，" + GlobalConfig.Ins.Name;
            else
                Name = "Hello World!";
        }
    }
}