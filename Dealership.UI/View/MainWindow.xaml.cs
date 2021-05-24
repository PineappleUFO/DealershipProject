using Dealership.UI.ViewModel;
using System.Windows;
using Dealership.SQL.Repository;

namespace Dealership.UI.View
{
    public partial class MainWindow : Window
    {
        public MainWindow(EnumSelectedPerson person)
        {
            InitializeComponent();
      

            this.DataContext = new MainViewModel(person,new PersonRepository().CurrentUser());
        }
    }
}
