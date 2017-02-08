using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WallE.Core;
using WallE.Core;
using WallE.Core.Extend;

namespace CommandExample
{
    public class CommandExample
    {
        private ICommand cmd;
        private AsyncCommand cmdAsync;

        private void CreateCommand()
        {
             cmd = new RelayCommand(OnAction, CanAction);
            ICommand cmd1 = new RelayCommand(OnActionArg, CanAction);
            ICommand cmd2 = new RelayCommand(OnAction);
            cmdAsync = new AsyncCommand(OnAction, CanActionAsync);
        }

        private void RegiestGlobalCommand()
        {
            M.CommandManager.Register(new CommandInfo("CommandName", cmd));
            M.CommandManager.Register(new CommandInfo("CommandName", cmdAsync.Command));
        }
        private void UnRegiestGlobalCommand()
        {
            M.CommandManager.Unregister("CommandName");
        }
        private void GetGlobalCommand()
        {
           CommandInfo cmdInfo=  M.CommandManager.GetCommandInfo("CommandName");
            ICommand cmdtemp = cmdInfo.Command;
        }
        private void OnAction()
        {
        }
        private void OnActionArg(object o)
        {
        }
        private bool CanAction()
        {
            return true;
        }

        private async Task<bool> CanActionAsync()
        {
            return true;
        }
    }
}
