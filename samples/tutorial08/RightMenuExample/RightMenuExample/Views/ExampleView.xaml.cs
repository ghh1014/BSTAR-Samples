
using Telerik.Windows;
using WallE.Core;

namespace RightMenuExample.Views
{
    /// <summary>
    /// Interaction logic for ExampleView.xaml
    /// </summary>
    public partial class ExampleView
    {
        public ExampleView()
        {
            InitializeComponent();
        }

        private void RadContextMenu_OnOpening(object sender, RadRoutedEventArgs e)
        {
            Messager.Send(this, new Message<RadRoutedEventArgs>("MenuOpening", e));
        }
    }
}
