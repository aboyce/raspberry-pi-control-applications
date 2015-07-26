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
using RasPi_Controller.Extension_Methods;
using RasPi_Controller.Models;
using RasPi_Controller.ViewModels;
using RasPi_Controller.Helpers;

namespace RasPi_Controller
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly RasPiControllerWindowViewModel _vm;

        public MainWindow()
        {
            InitializeComponent();
            _vm = new RasPiControllerWindowViewModel();

            DataContext = _vm;
        }

#region Buttons

        private void BtnSaveConfig_Click(object sender, RoutedEventArgs e)
        {
            if (_vm.SaveToConfiguration())
                MessageBox.Show("Saved to Config File", "Success");
            else
                MessageBox.Show("Could not save to config", "Error");
        }

        private void BtnTestNetworkName_Click(object sender, RoutedEventArgs e)
        {
            TextRange content = new TextRange(TbxRasPiNetworkName.Document.ContentStart, TbxRasPiNetworkName.Document.ContentEnd);
            string pingResult = NetworkingHelper.TryToPing(content.Text);

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
            string pingResult = NetworkingHelper.TryToPing(content.Text);

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
            
        }
        private void BtnSend_Click(object sender, RoutedEventArgs e)
        {
        }
        private void BtnLoadConfig_Click(object sender, RoutedEventArgs e)
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