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
using NFC_Card_Reader.Peripheral;

namespace NFC_Card_Reader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private CardReader reader;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnInitialise_Click(object sender, RoutedEventArgs e)
        {
            reader = new CardReader();
            string[] result = reader.PopulateReaders();

            if (result == null)
            {
                MessageBox.Show("Cannot connect to any readers", "Error");
                return;
            }

            LbxReaders.ItemsSource = result;
            LbxReaders.IsEnabled = IsEnabled;
        }

        private void LbxReaders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BtnMonitor.IsEnabled = IsEnabled;
        }

        private void BtnMonitor_Click(object sender, RoutedEventArgs e)
        {
            var selected = LbxReaders.SelectedItem;
            if (selected == null) return;

            string result = reader.StartMonitoringSelectedReader(selected.ToString());

            if (result == null)
            {
                MessageBox.Show(string.Format("Now monitoring {0}", selected.ToString()));
                _monitoring();
            }
            else
                MessageBox.Show(result);

        }

        private void BtnGetStatus_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(reader.GetStatus(), "Result");
        }

#region UI Changes

        private void _initialised()
        {
            BtnMonitor.IsEnabled = IsEnabled;
            LbxReaders.IsEnabled = IsEnabled;
        }

        private void _monitoring()
        {
            BtnGetStatus.IsEnabled = IsEnabled;
        }

        private void _disconncted()
        {
            LbxReaders.ItemsSource = null;

            BtnGetStatus.IsEnabled = false;
            BtnMonitor.IsEnabled = false;
        }

#endregion

    }
}
