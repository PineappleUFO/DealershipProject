using Dealership.UI.ViewModel;
using System.Windows;

namespace Dealership.UI.View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = new MainViewModel();
        }
    }
}
