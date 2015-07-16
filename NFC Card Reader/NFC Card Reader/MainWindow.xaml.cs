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
            BtnConnect.IsEnabled = IsEnabled;
        }

        private void BtnConnect_Click(object sender, RoutedEventArgs e)
        {
            var selected = LbxReaders.SelectedItem;
            if (selected == null) return;

            string result = reader.ConnectToReader(selected.ToString());

            if (result == null)
            {
                MessageBox.Show(string.Format("Connected to {0}", selected.ToString()));
                _connected();
            }
            else
                MessageBox.Show(result);

        }

        private void BtnDisconnect_Click(object sender, RoutedEventArgs e)
        {
            reader.Disconnect();
            _disconncted();
        }

        private void BtnGetProtocol_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(reader.GetProtocol(), "Protocol");
        }

        private void _initialised()
        {
            BtnConnect.IsEnabled = IsEnabled;
            LbxReaders.IsEnabled = IsEnabled;
        }

        private void _connected()
        {
            BtnDisconnect.IsEnabled = IsEnabled;
            BtnGetProtocol.IsEnabled = IsEnabled;
            BtnGetStatus.IsEnabled = IsEnabled;
        }

        private void _disconncted()
        {
            LbxReaders.ItemsSource = null;

            BtnDisconnect.IsEnabled = false;
            BtnGetProtocol.IsEnabled = false;
            BtnGetStatus.IsEnabled = false;
            BtnConnect.IsEnabled = false;
        }

        private void BtnGetStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(reader.GetStatus(), "Result");
        }

        private void BtnListen_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
