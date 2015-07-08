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

        private void BtnLoadConfig_Click(object sender, RoutedEventArgs e)
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
                EnableAll();
                BtnLoadConfig.Content = "Re-Load Config";
                BtnLoadConfig.Background = Brushes.FloralWhite;
            }

            LbxRasPis.ItemsSource = _model.RaspberryPis;
            LbxScripts.ItemsSource = _model.Scripts;

        }

        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Get everything validated and then send command.
        }

        private void BtnTestNetworkName_Click(object sender, RoutedEventArgs e)
        {
            string pingResult = _logic.TryToPing(new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd).Text);

            if (pingResult == null)
            {
                // TODO: Make the text green
                MessageBox.Show("Success");
            }
            else
            {
                // TODO: Make the text red
                MessageBox.Show(pingResult, "Error");
            }
        }

        private void BtnTestIpAddress_Click(object sender, RoutedEventArgs e)
        {
            string pingResult = _logic.TryToPing(new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd).Text);

            if (pingResult == null)
            {
                // TODO: Make the text green
                MessageBox.Show("Success");
                TbxRasPiIpAddress.Selection.ApplyPropertyValue(ForegroundProperty, Brushes.Green);
            }
            else
            {
                TbxRasPiIpAddress.Selection.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                MessageBox.Show(pingResult, "Error");
            }


        }

        private void LbxRasPis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbxRasPis.SelectedItem == null) return;
            RaspberryPi rasPi = (RaspberryPi)LbxRasPis.SelectedItem;
            TbxRasPiNetworkName.Document.Blocks.Clear();
            TbxRasPiNetworkName.Document.Blocks.Add(new Paragraph(new Run(rasPi.NetworkName)));
            TbxRasPiIpAddress.Document.Blocks.Clear();
            TbxRasPiIpAddress.Document.Blocks.Add(new Paragraph(new Run(rasPi.IpAddress)));
            TbxRasPiUsername.Text = rasPi.Username;
        }

        private void LbxScripts_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbxScripts.SelectedItem == null) return;
            Script script = (Script)LbxScripts.SelectedItem;
            TbxScriptName.Text = script.Name;
            TbxScriptDescription.Text = script.Description;
            TbxScriptArgumentFormat.Text = script.ArgumentFormat;
        }

        private void TbxRasPiIpAddress_TextChanged(object sender, TextChangedEventArgs e)
        {
            //TbxRasPiIpAddress.Selection.ApplyPropertyValue(RichTextBox.ForegroundProperty, Brushes.Black);
        }

        private void EnableAll()
        {
            LbRasPis.IsEnabled = IsEnabled;
            LbxRasPis.IsEnabled = IsEnabled;
            LbScripts.IsEnabled = IsEnabled;
            LbxScripts.IsEnabled = IsEnabled;

            LbRasPiHeader.IsEnabled = IsEnabled;
            LbRasPiNetworkName.IsEnabled = IsEnabled;
            TbxRasPiNetworkName.IsEnabled = IsEnabled;
            LbRasPiIpAddress.IsEnabled = IsEnabled;
            TbxRasPiIpAddress.IsEnabled = IsEnabled;
            LbRasPiUsername.IsEnabled = IsEnabled;
            TbxRasPiUsername.IsEnabled = IsEnabled;
            LbRasPiPassword.IsEnabled = IsEnabled;
            PwPasPiPassword.IsEnabled = IsEnabled;

            LbScriptHeader.IsEnabled = IsEnabled;
            LbScriptName.IsEnabled = IsEnabled;
            TbxScriptName.IsEnabled = IsEnabled;
            LbScriptArguments.IsEnabled = IsEnabled;
            TbxScriptArguments.IsEnabled = IsEnabled;
            LbScriptDescription.IsEnabled = IsEnabled;
            TbxScriptDescription.IsEnabled = IsEnabled;
            LbScriptArgumentFormat.IsEnabled = IsEnabled;
            TbxScriptArgumentFormat.IsEnabled = IsEnabled;

            BtnSend.IsEnabled = IsEnabled;
            BtnTestIpAddress.IsEnabled = IsEnabled;
            BtnTestNetworkName.IsEnabled = IsEnabled;
        }
    }
}