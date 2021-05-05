using System.Windows.Controls;

namespace Dealership.UI.View.MenuView
{
    /// <summary>
    /// Логика взаимодействия для MenuControl.xaml
    /// </summary>
    public partial class MenuControl : UserControl
    {
        public MenuControl()
        {
            InitializeComponent();
            this.DataContext = new MenuViewModel();
        }
    }
}
