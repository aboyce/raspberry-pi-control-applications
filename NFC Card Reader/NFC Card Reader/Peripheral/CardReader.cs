using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using PCSC;
using PCSC.Iso7816;

// Using https://github.com/danm-de/pcsc-sharp  // Library guide https://danm.de/docs/pcsc-sharp/PCSC/index.html

namespace NFC_Card_Reader.Peripheral
{
    class CardReader
    {
        private string[] _readers;
        private string[] _monitoredReaders;
        private SCardMonitor _monitor;
        private string _connectedReader = string.Empty;

        /// <summary>
        /// Tries to populate the list of readers and returns it.
        /// </summary>
        /// <returns>Null if an error occurs. The populated string[] is successful.</returns>
        public string[] PopulateReaders()
        {
            SCardContext context = new SCardContext();
            context.Establish(SCardScope.System);

            try
            {
                _readers = context.GetReaders();
            }
            catch (Exception)
            {
                if (_readers == null)
                    return null;
            }

            context.Dispose();
            return _readers;
        }

        /// <summary>
        /// Tries to connect to the selected reader.
        /// </summary>
        /// <returns>Null if successful. The error message if not.</returns>
        public string StartMonitoringSelectedReader(string readerName)
        {
            if (string.IsNullOrEmpty(readerName)) return "Reader name is null or empty";
            if (!_readers.Contains(readerName)) return "The reader does not exist. [Logic Error]";
            _connectedReader = readerName;

            SCardContext context = new SCardContext();
            context.Establish(SCardScope.System);
            SCardReader reader = new SCardReader(context);

            SCardError result = SCardError.InternalError;

            try
            {
                result = reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any);
            }
            catch (Exception)
            {
                context.Dispose();
                reader.Dispose();
                return SCardHelper.StringifyError(result);
            }

            _monitor = new SCardMonitor(new SCardContext(), SCardScope.System, true);
            _monitor.Initialized += (_cardInitalised);
            _monitor.CardInserted += (_cardInserted);
            _monitor.CardRemoved += (_cardRemoved);
            _monitor.Start(readerName);

            return null;
        }

        private void _cardInitalised(object sender, CardStatusEventArgs e)
        {
            string message = string.Empty;

            if (string.IsNullOrEmpty(_connectedReader) || _monitor == null) return;
            if (_monitor.Monitoring)
            {
                _monitoredReaders = _monitor.ReaderNames;
                string monitoredReaders = _monitor.ReaderNames.Aggregate(string.Empty, (current, reader) => current + " " + reader);
                message = string.Format("Now monitoring {0}", monitoredReaders);
                // TODO: Update the UI
            }
            else
            {
                message = "Not monitoring any card readers, won't detect that a new card is inserted.";
            }

            string result = message;
        }

        private void _cardInserted(object sender, CardStatusEventArgs e)
        {
            SCardContext context = new SCardContext();
            context.Establish(SCardScope.System);
            SCardReader reader = new SCardReader(context);

            SCardError result = reader.Connect(_connectedReader, SCardShareMode.Shared, SCardProtocol.Any);
            if (result != SCardError.Success)
            {
                context.Dispose();
                reader.Dispose();
                // TODO: Get error to UI
                string errorForUi = SCardHelper.StringifyError(result);
            }
            
            CommandApdu apdu = new CommandApdu(IsoCase.Case2Short, reader.ActiveProtocol)
            {
                CLA = 0xFF,
                Instruction = InstructionCode.GetData,
                P1 = 0x00,
                P2 = 0x00,
                Le = 0
            };

            result = reader.BeginTransaction();
            if (result != SCardError.Success)
            {
                context.Dispose();
                reader.Dispose();
                // TODO: Get error to UI
                string errorForUi = SCardHelper.StringifyError(result);
            }

            var recievePci = new SCardPCI();
            var sendPci = SCardPCI.GetPci(reader.ActiveProtocol);

            byte[] recieveBuffer = new byte[256];

            result = reader.Transmit(sendPci, apdu.ToArray(), recievePci, ref recieveBuffer);
            if (result != SCardError.Success)
            {
                context.Dispose();
                reader.Dispose();
                // TODO: Get error to UI
                string errorForUi = SCardHelper.StringifyError(result);
            }

            var responseApdu = new ResponseApdu(recieveBuffer, IsoCase.Case2Short, reader.ActiveProtocol);

            string SW1 = responseApdu.SW1.ToString();
            string SW2 = responseApdu.SW2.ToString();
            string Uid;

            if (responseApdu.HasData)
            {
                Uid = BitConverter.ToString(responseApdu.GetData());
            }

            reader.EndTransaction(SCardReaderDisposition.Leave);
            reader.Dispose();
            context.Dispose();
        }

        private void _cardRemoved(object sender, CardStatusEventArgs e)
        {
            string test = "is this working";
        }

        //public void Disconnect()
        //{
        //    _context.Dispose();
        //    _reader.Dispose();
        //}
    }
}
