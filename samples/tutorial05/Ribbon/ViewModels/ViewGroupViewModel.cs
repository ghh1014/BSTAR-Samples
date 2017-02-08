using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WallE.Core;

namespace Ribbon.ViewModels
{
    public class ViewGroupViewModel:RibbonGroupViewModel
    {
        public ICommand Click { get; set; }

        public ViewGroupViewModel():base("Ribbons/ViewGroup.xml", "Assets")
        {
            Click=new RelayCommand(OnClick);
        }

        private void OnClick()
        {
        }
    }
}
