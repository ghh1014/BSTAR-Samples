using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using CommandExample.ViewModels;
using WallE.Core;
using WallE.Core.Extend;

namespace CommandExample
{
    public class MainPlugin : IPlugin
    {
        private DockingPaneViewModel _panel;
        private AsyncCommand alertMessageAsync;
        private ICommand alertMessage;
        public void Install(IPluginInfo pluginInfo)
        {
            CommandExampleViewModel commandExamleViewModel = new CommandExampleViewModel();
            _panel = new DockingPaneViewModel(commandExamleViewModel)
            {
                Header = "Command测试示例",
                IsActive = true,
                IsHidden = false,
                InitialPosition = DockPosition.FloatingDockable,
                IsDocument = true
            };
            M.DockingManager.InsertPane(_panel);

            //注册全局命令
             alertMessage = new RelayCommand(OnAlertMessage);
             alertMessageAsync = new AsyncCommand(OnAlertMessage, CanAlertMessageAsync);
            M.CommandManager.Register(new CommandInfo("AlertMessage", alertMessage));//全局命令可以任何地方使用
            M.CommandManager.Register(new CommandInfo("AlertMessageAsync", alertMessageAsync.Command));//AsyncCommand在注册全局命令是需要用其属性Command
        }

        public void Uninstall()
        {
            if (_panel != null)
                M.DockingManager.RemovePane(_panel);
        }

        private void OnAlertMessage()
        {
            MessageBox.Show("这个一个全局的命令");
        }
        private async Task<bool> CanAlertMessageAsync()
        {
            await Task.Run(() =>
            {
                Task.Delay(1000);
            });
            return true;
        }
    }
}
