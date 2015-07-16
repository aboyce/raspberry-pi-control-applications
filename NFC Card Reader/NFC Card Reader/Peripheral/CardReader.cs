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

        public delegate void CardInitializedEvent(object sender, CardStatusEventArgs e);


        public CardReader()
        {

        }

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

        public string[] GetReaders()
        {
            return _readers;
        }

        /// <summary>
        /// Tries to connect to the selected reader.
        /// </summary>
        /// <returns>Null if successful. The error message if not.</returns>
        public string ConnectToReader(string readerName)
        {
            if (string.IsNullOrEmpty(readerName)) return "Reader name is null or empty";
            string checks = _checks();
            if (checks != null)
                return checks;

            _reader = new SCardReader(_context);

            SCardError result = _reader.Connect(readerName, SCardShareMode.Shared, SCardProtocol.Any);

            return result == SCardError.Success ? null : SCardHelper.StringifyError(result);
        }

        public string GetConnectedReader()
        {
            return _reader.ReaderName;
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

        //public bool Listen()
        //{
        //    //SCardMonitor monitor = new SCardMonitor(new SCardContext(), SCardScope.System);
        //    //monitor.Initialized += new CardInitializedEvent(_CardInitialized);
        //    //monitor.Start("");

        //}

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
