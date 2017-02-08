using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WallE.Core;
using WallE.Core.Extend;

namespace CommandExample.ViewModels
{
    public class CommandExampleViewModel:ViewModelBase
    {
        #region Private Field

        private bool _isChecked;

        #endregion

        #region Public Property

        public bool IsChecked
        {
            get { return _isChecked; }
            set { Set("IsChecked", ref _isChecked, value); }
        }
        #endregion

        #region Construction

        public CommandExampleViewModel()
        {
            //插件类的命令绑定
            Click = new RelayCommand(OnClick);
            ClickEnabled=new RelayCommand(OnClickEnabled, CanClickEnabled);
            ClickEnabledAsync=new AsyncCommand(OnClick,CanClickAsync);//异步的方法
            RegisterGlobalCommand=new RelayCommand(OnRegisterGlobalCommand);
            //GlobalCommand = new RelayCommand(OnAlertMessage);
        }

        #endregion

        #region Command/Event

        public ICommand Click { get; set; }
        private void OnClick()
        {
            _isChecked = !_isChecked;
        }
        public ICommand ClickEnabled { get; set; }



        private void OnClickEnabled()
        {
          
        }
        private bool CanClickEnabled()
        {
            return _isChecked;
        }

        public AsyncCommand ClickEnabledAsync { get; set; }

        private async Task<bool> CanClickAsync()
        {
            await Task.Run(() =>
            {
                Task.Delay(1000);
            });
            _isChecked = !_isChecked;
            return _isChecked;
        }

        public ICommand RegisterGlobalCommand { get; set; }

        private ICommand _globalCommand;
        public ICommand GlobalCommand {
            get { return _globalCommand; }
            set { Set("GlobalCommand", ref _globalCommand, value); }//这里将命令设为通知的方式GlobalCommand，才会有效 
        }
        private void OnRegisterGlobalCommand()
        {
              GlobalCommand= M.CommandManager.GetCommandInfo("AlertMessage").Command;
        }

        #endregion

        #region Override Method
        #endregion

        #region Private/Public Method



        #endregion
    }
}
