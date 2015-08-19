using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using NFC_Card_Reader.Commands;
using NFC_Card_Reader.Helpers;
using NFC_Card_Reader.Models;

namespace NFC_Card_Reader.ViewModels
{
    public class ReaderViewModel : INotifyPropertyChanged
    {
        private ObservableCollection<CardReader> _cardReaders;
        public ObservableCollection<CardReader> CardReaders {
            get { return _cardReaders; }
            set { _cardReaders = value; NotifyPropertyChanged("CardReaders"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        public ICommand InitialiseCommand { get; set; }
        public ICommand MonitorCommand { get; set; }
        public ICommand GetStatusCommand { get; set; }
        public ICommand ReadCardCommand { get; set; }

        private CardReader _currentReader;
        public CardReader CurrentReader {
            get { return _currentReader; }
            set { _currentReader = value; NotifyPropertyChanged("CurrentReader"); } }

        public CardReaderHelper ReaderHelper;

        public ReaderViewModel()
        {
            ReaderHelper = new CardReaderHelper();
            
            InitialiseCommand = new InitialiseCommand(this);
            MonitorCommand = new MonitorCommand(this);
            GetStatusCommand = new GetStatusCommand(this);
            ReadCardCommand = new ReadCardCommand(this);
        }

        protected void NotifyPropertyChanged(string property)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(property));
        }

        /// <summary>
        /// Puts the header and message in a MessageBox.Show
        /// </summary>
        /// <param name="header">The message header</param>
        /// <param name="message">The message body</param>
        public void MessageToView(string header, string message)
        {
            MessageBox.Show(message, header);
        }
    }
}
