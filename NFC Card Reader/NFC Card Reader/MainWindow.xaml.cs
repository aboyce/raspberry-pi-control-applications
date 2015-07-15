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

        private void BtnStart_Click(object sender, RoutedEventArgs e)
        {
            reader = new CardReader();
            string result = reader.PopulateReaders();

            if (result != null)
            {
                MessageBox.Show(result, "Error");
                return;
            }

            result = reader.ConnectToReader();

            if (result != null)
            {
                MessageBox.Show(result, "Error");
                return;
            }

            MessageBox.Show(string.Format("Connected to {0}", reader.GetConnectedReader()));
            _connected();
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

        private void _connected()
        {
            BtnDisconnect.IsEnabled = IsEnabled;
            BtnGetProtocol.IsEnabled = IsEnabled;
        }

        private void _disconncted()
        {
            BtnDisconnect.IsEnabled = false;
            BtnGetProtocol.IsEnabled = false;
        }
    }
}
