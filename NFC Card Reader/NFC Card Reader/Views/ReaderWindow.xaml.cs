using System.Windows;
using System.Windows.Controls;
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader.Views
{
    /// <summary>
    /// Interaction logic for ReaderWindow.xaml
    /// </summary>
    public partial class ReaderWindow : Window
    {
        public ReaderWindow()
        {
            InitializeComponent();
            DataContext = new ReaderViewModel();
        }
    }
}
