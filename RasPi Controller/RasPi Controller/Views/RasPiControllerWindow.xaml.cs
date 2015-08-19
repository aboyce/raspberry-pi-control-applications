using System.Windows;
using RasPi_Controller.ViewModels;

namespace RasPi_Controller.Views
{

    public partial class RasPiControllerWindow : Window
    {
        public RasPiControllerWindow()
        {
            InitializeComponent();
            DataContext = new RasPiControllerWindowViewModel();
        }
    }
}