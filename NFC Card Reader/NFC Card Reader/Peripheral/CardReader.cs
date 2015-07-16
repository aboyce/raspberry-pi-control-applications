using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using PCSC; // Using https://github.com/danm-de/pcsc-sharp  // Library guide https://danm.de/docs/pcsc-sharp/PCSC/index.html

namespace NFC_Card_Reader.Peripheral
{
    class CardReader
    {
        private string[] _readers;
        private SCardContext _context;
        private SCardReader _reader;
        private SCardMonitor _monitor;
        private SCardProtocol _protocol = SCardProtocol.Unset;

        private string _checks()
        {
            if (_context == null) return "The context is not set";

            if (!_readers.Any()) return "There are no readers loaded";

            if (_readers.Count() > 1) return "Currently cannot handle more that one reader";

            return null;
        }

        /// <summary>
        /// Tries to populate the list of readers and returns it.
        /// </summary>
        /// <returns>Null if an error occurs. The populated string[] is successful.</returns>
        public string[] PopulateReaders()
        {
            _context = new SCardContext();
            _context.Establish(SCardScope.System);

            try
            {
                _readers = _context.GetReaders();
            }
            catch (Exception)
            {
                if (_readers == null)
                    return null;
            }

            return _readers;
        }

        public string GetConnectedReader()
        {
            return _reader != null ? _reader.ReaderName : null;
        }

        /// <summary>
        /// Tries to connect to the selected reader.
        /// </summary>
        /// <returns>Null if successful. The error message if not.</returns>
        public string SetSelectedReader(string readerName)
        {
            if (string.IsNullOrEmpty(readerName)) return "Reader name is null or empty";
            if (!_readers.Contains(readerName)) return "The reader does not exist. [Logic Error]";
            string checks = _checks();
            if (checks != null)
                return checks;

            _reader = new SCardReader(_context);

            _reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any);

            Setup(readerName);

            return null;

            //return result == SCardError.Success ? null : SCardHelper.StringifyError(result);
        }

        public bool Setup(string readerName)
        {
            _monitor = new SCardMonitor(new SCardContext(), SCardScope.System, true);
            _monitor.Initialized += (_cardInitalised);
            _monitor.CardInserted += (_cardInserted);
            _monitor.CardRemoved += (_cardRemoved);
            _monitor.Start(readerName);

            return true;
        }

        public string GetProtocol()
        {
            string checks = _checks();
            if (checks != null)
                return checks;

            switch (_reader.ActiveProtocol)
            {
                    case SCardProtocol.T0:
                        _protocol = SCardProtocol.T0;
                        break;
                    case SCardProtocol.T1:
                        _protocol = SCardProtocol.T1;
                        break;
                    case SCardProtocol.T15:
                        _protocol = SCardProtocol.T15;
                        break;
                    case SCardProtocol.Raw:
                        _protocol = SCardProtocol.Raw;
                        break;
                    case SCardProtocol.Any:
                        _protocol = SCardProtocol.Any;
                        break;
                    case SCardProtocol.Unset:
                        _protocol = SCardProtocol.Unset;
                        break;
                default:
                    return "No supported protocol";
            }

            return _protocol.ToString();
        }

        public string GetStatus()
        {
            string checks = _checks();
            if (checks != null)
                return checks;

            string[] name;
            SCardState state;
            SCardProtocol protocol;
            byte[] array;

            SCardError result = _reader.Status(out name, out state, out protocol, out array);

            return result != SCardError.Success ? null : array.ToString();
        }

        private void _cardInitalised(object sender, CardStatusEventArgs e)
        {
            string message = string.Empty;

            if (_reader == null || _monitor == null) return;
            if (_monitor.Monitoring)
            {
                message = string.Format("Now monitoring {0}", _reader.ReaderName);
            }
        }
        private void _cardInserted(object sender, CardStatusEventArgs e)
        {
            string test = "is this working";
        }
        private void _cardRemoved(object sender, CardStatusEventArgs e)
        {
            string test = "is this working";
        }



        //public void _CardInitialized(object sender, CardEventArgs e)
        //{

        //}

        public void Disconnect()
        {
            _context.Dispose();
            _reader.Dispose();
        }
    }
}
