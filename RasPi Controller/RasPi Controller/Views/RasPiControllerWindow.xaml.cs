using System.Windows;
using RasPi_Controller.ViewModels;

namespace RasPi_Controller.Views
{
    /// <summary>
    /// Interaction logic for RasPiControllerWindow.xaml
    /// </summary>
    public partial class RasPiControllerWindow : Window
    {
        public RasPiControllerWindow()
        {
            InitializeComponent();
            DataContext = new RasPiControllerWindowViewModel();
        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnSaveScript_Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnSaveRasPi_Click(object sender, RoutedEventArgs e)
        {
        }
    }
}