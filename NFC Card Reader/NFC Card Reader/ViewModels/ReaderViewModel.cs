using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    public class ReaderViewModel
    {
        public ObservableCollection<CardReader> CardReaders;

        public ICommand InitialiseCommand;
        public ICommand MonitorCommand;
        public ICommand GetStatusCommand;
        public ICommand ReadCardCommand;

        public string ReaderName { get; set; }

        public CardReaderHelper ReaderHelper;

        public ReaderViewModel()
        {
            ReaderHelper = new CardReaderHelper();
            
            InitialiseCommand = new InitialiseCommand(this);
            MonitorCommand = new MonitorCommand(this);
            GetStatusCommand = new GetStatusCommand(this);
            ReadCardCommand = new ReadCardCommand(this);
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
