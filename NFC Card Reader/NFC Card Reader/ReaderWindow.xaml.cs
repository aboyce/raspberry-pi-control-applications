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
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader
{
    /// <summary>
    /// Interaction logic for ReaderWindow.xaml
    /// </summary>
    public partial class ReaderWindow : Window
    {
        private ReaderViewModel _rvm;

        public ReaderWindow()
        {
            InitializeComponent();

            _rvm = new ReaderViewModel();

            this.DataContext = _rvm;
        }

        private void BtnInitialise_Click(object sender, RoutedEventArgs e)
        {
            
            if (!_rvm.GetReaders())
            {
                MessageBox.Show("Cannot connect to any readers", "Error");
                return;
            }

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

            bool result = _rvm.MonitorReader(selected.ToString());

            if (result)
            {
                MessageBox.Show(string.Format("Now monitoring {0}", selected.ToString()));
                _monitoring();
            }
            else
                MessageBox.Show("Cannot monitor reader");

        }

        private void BtnGetStatus_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(reader.GetReaderStatus(), "State");
        }

        private void BtnReadCard_Click(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(reader.ReadCard(), "Read Card");
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
            BtnReadCard.IsEnabled = IsEnabled;
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
