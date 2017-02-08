using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WallE.Core;

namespace Ribbon.ViewModels
{
    public class ViewButtonViewModel:RibbonGroupViewModel
    {
        public ICommand Click { get; set; }

        public ViewButtonViewModel(): base("Ribbons/ViewButton.xml", "Assets")
        { 

        }

        private void OnClick()
        {
        }
    }
}
