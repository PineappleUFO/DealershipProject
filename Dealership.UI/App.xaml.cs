using System.Windows;
using Dealership.UI.View;

namespace Dealership.UI
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            SelectPerson person = new SelectPerson();
            person.Show();

            base.OnStartup(e);
        }
    }
}
