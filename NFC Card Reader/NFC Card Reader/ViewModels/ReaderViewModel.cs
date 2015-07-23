using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NFC_Card_Reader.Helpers;
using NFC_Card_Reader.Models;

namespace NFC_Card_Reader.ViewModels
{
    public class ReaderViewModel
    {
        public ObservableCollection<CardReader> CardReaders;

        public string ReaderName { get; set; }

        private CardReaderHelper _readerHelper;

        public ReaderViewModel()
        {
            _readerHelper = new CardReaderHelper();
            CardReaders = new ObservableCollection<CardReader>();
        }

        public bool GetReaders()
        {
            string[] stringReaders = _readerHelper.PopulateReaders();

            foreach (string readerName in stringReaders)
            {
                CardReaders.Add(new CardReader(readerName));
            }

            if (CardReaders.Count > 0)
                return true;
            else
                return false;
        }

        public bool MonitorReader(string reader)
        {
            if (_readerHelper.StartMonitoringSelectedReader(reader) == null)
                return true;
            else
                return false;
        }


    }
}
