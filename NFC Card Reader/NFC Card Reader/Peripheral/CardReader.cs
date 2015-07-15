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
        private SCardProtocol _protocol = SCardProtocol.Unset;

        private string _checks()
        {
            if (_context == null) return "The context is not set";

            if (!_readers.Any()) return "There are no readers loaded";

            if (_readers.Count() > 1) return "Currently cannot handle more that one reader";

            return null;
        }

        /// <summary>
        /// Tries to populate the list of readers.
        /// </summary>
        /// <returns>The error is one occurs. Null if successful.</returns>
        public string PopulateReaders()
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
                    return "Could not find any readers";
            }

            return null;
        }

        public string[] GetReaders()
        {
            return _readers;
        }

        public string ConnectToReader()
        {
            string checks = _checks();
            if (checks != null)
                return checks;

            _reader = new SCardReader(_context);

            SCardError result = _reader.Connect(_readers[0], SCardShareMode.Shared, SCardProtocol.Any);

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

        public void Disconnect()
        {
            _context.Dispose();
            _reader.Dispose();
        }


    }
}
