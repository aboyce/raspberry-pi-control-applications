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
using System.Windows.Threading;
using System.Xaml;
using RasPi_Controller.Models;
using RasPi_Controller.ViewModels;

namespace RasPi_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainWindowViewModel _vm;


        public MainWindow()
        {
            InitializeComponent();
        }

#region Buttons

        private void BtnLoadConfig_Click(object sender, RoutedEventArgs e)
        {
            _vm = new MainWindowViewModel();
            EnableAll();
            BtnLoadConfig.Content = "Re-Load Config";
            BtnLoadConfig.Background = Brushes.FloralWhite;

            LbxRasPis.ItemsSource = _vm.RaspberryPis;
            LbxScripts.ItemsSource = _vm.Scripts;
        }

        private void BtnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SaveToConfiguration())
                MessageBox.Show("Saved to Config File", "Success");
            else
                MessageBox.Show("Could not save to config", "Error");
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
            string pingResult = _vm.TryToPing(content.Text);

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

        private void BtnTestIpAddress_Click(object sender, RoutedEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            string pingResult = _vm.TryToPing(content.Text);

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

        private void BtnSaveRasPi_Click(object sender, RoutedEventArgs e)
        {
            TextRange idContent = new TextRange(TbxRasPiId.Document.ContentStart, TbxRasPiId.Document.ContentEnd);
            string id = _vm.CleanString(idContent.Text);
            TextRange networkNameContent = new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd);
            string networkName = _vm.CleanString(networkNameContent.Text);
            TextRange ipAddressContent = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            string ipAddress = _vm.CleanString(ipAddressContent.Text);
            string username = TbxRasPiUsername.Text;

            if (!_vm.CheckRaspberryPiIdIsUnique(id))
            {
                MessageBox.Show("The Id already exists", "Error");
                TextRange content = new TextRange(TbxRasPiId.Document.ContentStart, TbxRasPiId.Document.ContentEnd);
                content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                return;
            }

            if (networkName == string.Empty || ipAddress == string.Empty || username == string.Empty)
            {
                MessageBox.Show("Cannot save Raspberry Pi as there is missing content, please fill in relevant fields.", "Error");
            }
            else
            {
                _vm.RaspberryPis.Add(new RaspberryPi { Id = id, NetworkName = networkName, IpAddress = ipAddress, Username = username });
                MessageBox.Show("Raspberry Pi Added", "Success");
            }

            LbxRasPis.Items.Refresh();
        }

        private void BtnSaveScript_Click(object sender, RoutedEventArgs e)
        {
            TextRange idContent = new TextRange(TbxScriptId.Document.ContentStart, TbxScriptId.Document.ContentEnd);
            string id = _vm.CleanString(idContent.Text);
            string name = TbxScriptName.Text;
            string description = TbxScriptDescription.Text;
            string argumentsFormat = TbxScriptArgumentFormat.Text;

            if (!_vm.CheckScriptIdIsUnique(id))
            {
                MessageBox.Show("The Id already exists", "Error");
                TextRange content = new TextRange(TbxScriptId.Document.ContentStart, TbxScriptId.Document.ContentEnd);
                content.ApplyPropertyValue(ForegroundProperty, Brushes.Red);
                return;
            }

            if (name == string.Empty || description == string.Empty || argumentsFormat == string.Empty)
            {
                MessageBox.Show("Cannot save Script as there is missing content, please fill in all relevant fields (including 'Arguments Format').", "Error");
            }
            else
            {
                _vm.Scripts.Add(new Script { Id = id, Name = name, Description = description, ArgumentFormat = argumentsFormat });
                MessageBox.Show("Script Added", "Success");
            }

            LbxScripts.Items.Refresh();
        }

#endregion

#region Changed Text

        private void TbxRasPiNetworkName_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd);
            content.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
        }

        private void TbxRasPiIpAddress_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiIpAddress.Document.ContentStart, TbxRasPiIpAddress.Document.ContentEnd);
            content.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
        }

        private void TbxRasPiId_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiId.Document.ContentStart, TbxRasPiId.Document.ContentEnd);
            content.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
        }

        private void TbxScriptId_KeyDown(object sender, KeyEventArgs e)
        {
            TextRange content = new TextRange(TbxScriptId.Document.ContentStart, TbxScriptId.Document.ContentEnd);
            content.ApplyPropertyValue(ForegroundProperty, Brushes.Black);
        }

#endregion

#region Selection Changed

        private void LbxRasPis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LbxRasPis.SelectedItem == null) return;
            RaspberryPi rasPi = (RaspberryPi)LbxRasPis.SelectedItem;
            TbxRasPiId.Document.Blocks.Clear();
            TbxRasPiId.Document.Blocks.Add(new Paragraph(new Run(rasPi.Id)));
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
            TbxScriptId.Document.Blocks.Clear();
            TbxScriptId.Document.Blocks.Add(new Paragraph(new Run(script.Id)));
            TbxScriptName.Text = script.Name;
            TbxScriptDescription.Text = script.Description;
            TbxScriptArgumentFormat.Text = script.ArgumentFormat;
        }

#endregion

        private void EnableAll()
        {
            LbRasPis.IsEnabled = IsEnabled;
            LbxRasPis.IsEnabled = IsEnabled;
            LbScripts.IsEnabled = IsEnabled;
            LbxScripts.IsEnabled = IsEnabled;

            LbRasPiHeader.IsEnabled = IsEnabled;
            LbRasPiId.IsEnabled = IsEnabled;
            TbxRasPiId.IsEnabled = IsEnabled;
            LbRasPiNetworkName.IsEnabled = IsEnabled;
            TbxRasPiNetworkName.IsEnabled = IsEnabled;
            LbRasPiIpAddress.IsEnabled = IsEnabled;
            TbxRasPiIpAddress.IsEnabled = IsEnabled;
            LbRasPiUsername.IsEnabled = IsEnabled;
            TbxRasPiUsername.IsEnabled = IsEnabled;
            LbRasPiPassword.IsEnabled = IsEnabled;
            PwPasPiPassword.IsEnabled = IsEnabled;

            LbScriptHeader.IsEnabled = IsEnabled;
            LbScriptId.IsEnabled = IsEnabled;
            TbxScriptId.IsEnabled = IsEnabled;
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
            BtnSaveRasPi.IsEnabled = IsEnabled;
            BtnSaveScript.IsEnabled = IsEnabled;
            BtnSaveConfig.IsEnabled = IsEnabled;
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