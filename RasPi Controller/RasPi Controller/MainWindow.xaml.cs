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
using System.Xaml;

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

            // TODO: Check network name and ip address, if possible pick ip address.
            // TODO: Check username is valid.
            // TODO: See if passwords are required, if so check this, if not it should be fine.
            // TODO: Check the script name is valid.
            // TODO: Return the result.
        }

        private void BtnTestNetworkName_Click(object sender, RoutedEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd);
            string pingResult = _logic.TryToPing(content.Text);

            if (pingResult == null)
            {
                content.ApplyPropertyValue(ForegroundProperty, Brushes.Green);
            }
            else
            {
                content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                MessageBox.Show(pingResult, "Error");
            }
        }

        private void TbxRasPiNetworkName_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd);
            content.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
        }

        private void BtnTestIpAddress_Click(object sender, RoutedEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            string pingResult = _logic.TryToPing(content.Text);

            if (pingResult == null)
            {
                content.ApplyPropertyValue(ForegroundProperty, Brushes.Green);
            }
            else
            {
                content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                MessageBox.Show(pingResult, "Error");
            }
        }

        private void TbxRasPiIpAddress_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            content.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
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

        private void BtnHelp_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(string.Format("This application is indented to send SSH commands to a remote location. {0}{0}" +
                                          "The minimal requirements are; {0}" +
                                          " - Network Name or IP Address {0}" +
                                          " - Username/Password and Script Name/SSH Command. {0}{0}" +
                                          "The 'Test' buttons will ping the relevant field, and return either green or red with error. {0}{0}" +
                                          "The arguments can be left blank, but if present, will be added to the end of the script/command. {0}{0}" +
                                          "'Arguments Format' and 'Script Description' are both just for information but may be useful to other users. {0}{0}" +
                                          "You can store both Raspberry Pi (remote machine) and Script information in the XML file that is referenced in the .config file, this is to save retyping the same data. {0}" +
                                          "[Ensure that the XML file follows the examples formatting, and is valid XML] {0}{0}" +
                                          "Any queries contact via https://github.com/aboyce"
                                          , Environment.NewLine), "Help", MessageBoxButton.OK);
        }
    }
}