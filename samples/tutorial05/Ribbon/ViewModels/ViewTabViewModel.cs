using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WallE.Core;

namespace Ribbon.ViewModels
{
    public class ViewTabViewModel:RibbonTabViewModel
    {
        public ICommand Click { get; set; }
        public ViewTabViewModel() : base("Ribbons/ViewTab.xml", "Assets")
        {
            Click=new RelayCommand(OnClick);
        }
        private void OnClick()
        {
        }
    }
}
