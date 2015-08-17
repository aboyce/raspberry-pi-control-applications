using System.Windows;
using WebServerSimulator.ViewModels;

namespace WebServerSimulator.Views
{
    /// <summary>
    /// Interaction logic for WebServerSimulator.xaml
    /// </summary>
    public partial class WebServerSimulator : Window
    {
        public WebServerSimulator()
        {
            InitializeComponent();
            DataContext = new WebServerSimulatorViewModel();
        }
    }
}
