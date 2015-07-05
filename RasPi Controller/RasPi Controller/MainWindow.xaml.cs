using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RasPi_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowLogic _logic;
        private Model _model;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, RoutedEventArgs e)
        {
            _logic = new MainWindowLogic();
            _model = new Model();
            string loadInConfiguration = _model.LoadInConfiguration();
            if (loadInConfiguration != null)
            {
                MessageBox.Show(loadInConfiguration, "Error", MessageBoxButton.OK);
            }
            else
            {
                MessageBox.Show("Loaded in from configuration", "Loaded", MessageBoxButton.OK);
            }
        }

        private void btnSend_Click(object sender, RoutedEventArgs e)
        {



            //string returnval = SSHController.SendCommand("192.168.1.93", "remote", "RCoolingSystem", "sudo python PowerManager.py", "S R");
        }


    }
}