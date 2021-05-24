using System.Windows;

namespace Dealership.UI.View
{
    /// <summary>
    /// Логика взаимодействия для SelectPerson.xaml
    /// </summary>
    public partial class SelectPerson : Window
    {
        public SelectPerson()
        {
            InitializeComponent();
        }

        private void BtnClient_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPerson = EnumSelectedPerson.Client;
            MainWindow mainWindow = new MainWindow(selectedPerson);
            mainWindow.Show();
            this.Close();
        }

        private void BtnManager_OnClick(object sender, RoutedEventArgs e)
        {
            var selectedPerson = EnumSelectedPerson.Manager;
            MainWindow mainWindow = new MainWindow(selectedPerson);
            mainWindow.Show();
            this.Close();
        }
    }
}
