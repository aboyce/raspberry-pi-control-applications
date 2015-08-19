using System.Windows;
using System.Windows.Controls;
using NFC_Card_Reader.ViewModels;

namespace NFC_Card_Reader.Views
{
    public partial class ReaderWindow : Window
    {
        public ReaderWindow()
        {
            InitializeComponent();
            DataContext = new ReaderViewModel();
        }
    }
}
