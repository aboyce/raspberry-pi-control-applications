using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Documents;
using PCSC;
using PCSC.Iso7816;
using NFC_Card_Reader.Models;

// Using https://github.com/danm-de/pcsc-sharp
// Library guide https://danm.de/docs/pcsc-sharp/PCSC/index.html

namespace NFC_Card_Reader.Helpers
{
    public class CardReaderHelper
    {
        private string[] _readers;
        private SCardMonitor _monitor;
        private string _connectedReader = string.Empty;

        public bool AutoSend { get; set; }

        public ObservableCollection<CardReader> GetCardReaders()
        {
            ObservableCollection<CardReader> cardReaders = new ObservableCollection<CardReader>();

            string[] stringReaders = PopulateReaders();

            foreach (string readerName in stringReaders)
            {
                cardReaders.Add(new CardReader(readerName));
            }

            return cardReaders;
        }

        /// <summary>
        /// Tries to populate the list of readers and returns it.
        /// </summary>
        /// <returns>Null if an error occurs. The populated string[] is successful.</returns>
        public string[] PopulateReaders()
        {
            CardReader reader = new CardReader();
            if (!reader.Establish())
                return null;

            try
            {
                _readers = reader.GetReaders();
            }
            catch (Exception)
            {
                if (_readers == null)
                    return null;
            }

            reader.CleanUp();
            return _readers;
        }

        /// <summary>
        /// Tries to connect to the selected reader.
        /// </summary>
        /// <returns>Null if successful. The error message if not.</returns>
        public string StartMonitoringReader(string readerName)
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

        /// <summary>
        /// Checks to see if the readerName is being monitored
        /// </summary>
        /// <param name="readerName">The name of the reader to check</param>
        /// <returns>True if it being monitored. False if not.</returns>
        public bool IsMonitoringCardReader(string readerName)
        {
            return _monitor != null && _monitor.ReaderNames != null && _monitor.ReaderNames.Contains(readerName);
        }


        /// <summary>
        /// Will try to connect to the _connectedReader, see if there is a card present, and if so try to read the data.
        /// </summary>
        /// <returns>Either the error message or data from the card.</returns>
        public string TryToReadCard()
        {
            SCardContext context = new SCardContext();
            context.Establish(SCardScope.System);
            SCardReader reader = new SCardReader(context);

            SCardError result = SCardError.InternalError;

            try
            {
                result = reader.Connect(_connectedReader, SCardShareMode.Shared, SCardProtocol.Any);
            }
            catch (Exception)
            {
                context.Dispose();
                reader.Dispose();
                return SCardHelper.StringifyError(result);
            }

            string message;

            if (result == SCardError.Success)
            {
                string[] readerNames; SCardProtocol protocol; SCardState state; byte[] atr;
                result = reader.Status(out readerNames, out state, out protocol, out atr);

                if (result == SCardError.Success)
                    message = string.Format("Card detected:{0} Protocol: {1}{0} State: {2}{0} ATR: {3}",
                        Environment.NewLine, protocol, state, BitConverter.ToString(atr ?? new byte[0]));
                else
                    message = string.Format("Unable to read from card.{0}{1}", Environment.NewLine,
                        SCardHelper.StringifyError(result));
            }
            else
                message = string.Format("No card is detected (or reader reserved by another application){0} {1}",
                    Environment.NewLine, SCardHelper.StringifyError(result));

            context.Dispose();
            reader.Dispose();

            return message;
        }

        public string GetReaderStatus()
        {
            SCardContext context = new SCardContext();
            context.Establish(SCardScope.System);

            if (context.GetReaders().All(r => r != _connectedReader)) // Check to see if the context has _connectedReader as a reader before we use it.
            {
                context.Dispose();
                return string.Format("{0} cannot be found in the list of connected readers", _connectedReader);
            }

            var state = context.GetReaderStatus(_connectedReader);

            if (state == null)
            {
                context.Dispose();
                return string.Format("Cannot get the reader status of {0}.", _connectedReader);
            }

            context.Dispose();

            return string.Format("Name: {1}{0}Current State: {2}{0}Event State: {3}{0}" +
                                 "Current State Value: {4}{0}Event State Value: {5}{0}" +
                                 "User Data: {6}{0}Card Change Event Count: {7}{0}" +
                                 "ATR: {8}{0}", Environment.NewLine, state.ReaderName, state.CurrentState,
                                 state.EventState, state.CurrentStateValue, state.EventStateValue, state.UserData,
                                 state.CardChangeEventCnt, state.Atr.Length == 0 ? "0" : BitConverter.ToString(state.Atr));
        }

        private void _cardInitalised(object sender, CardStatusEventArgs e)
        {
        }

        private void _cardInserted(object sender, CardStatusEventArgs e)
        {
            if(AutoSend)
                Task.Run(() => { ReadCardAndSend(); });
        }

        private void _cardRemoved(object sender, CardStatusEventArgs e)
        {
        }

        /// <summary>
        /// Will call ReadCard to read the card, and then build and sent the SSH command to the SSH Client.
        /// </summary>
        /// <returns>Either the error message from itself/ReadCard or the SSH Client response.</returns>
        public string ReadCardAndSend()
        {
            string cardId = ReadCard(true);

            if (!cardId.Contains("UID^"))
                return cardId; // cardId will contain the error message from trying to read the card.

            cardId = cardId.Split('^', '^')[1];
            Configuration config = new Configuration();

            return config.Populate() ? SSHControllerHelper.SendCommand(config.Address, config.Username, config.Password, config.Script, string.Format(" {0} {1} {2}", config.ServerUrl, config.DoorId, cardId))
                : "The required credentials could not be collected from the .config file or application setting.";
        }

        /// <summary>
        /// Will try to connect to _connectedReader and read the card.
        /// </summary>
        /// <returns>Either the data from the card or the error message. Or if 'uidOnly' is true, just the UID prefixed with 'UID^' and ending with '^'</returns>
        public string ReadCard(bool uidOnly = false)
        {
            SCardContext context = new SCardContext();
            context.Establish(SCardScope.System);
            SCardReader reader = new SCardReader(context);
           
            SCardError result = reader.Connect(_connectedReader, SCardShareMode.Shared, SCardProtocol.Any);

            if (result != SCardError.Success)
            {
                context.Dispose();
                reader.Dispose();
                return string.Format("No card is detected (or reader reserved by another application){0}{1}",
                    Environment.NewLine, SCardHelper.StringifyError(result));
            }
            
            string[] readerNames; SCardProtocol protocol; SCardState state; byte[] atr;
            result = reader.Status(out readerNames, out state, out protocol, out atr);

            if (result != SCardError.Success)
            {
                context.Dispose();
                reader.Dispose();
                return string.Format("Unable to read from card.{0}{1}", Environment.NewLine, SCardHelper.StringifyError(result));
            }

            string message = string.Format("Card detected:{0}Protocol: {1}{0}State: {2}{0}ATR: {3}{0}",
                Environment.NewLine, protocol, state, BitConverter.ToString(atr ?? new byte[0]));

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
                return string.Format("Cannot start transaction.{0} {1}", Environment.NewLine, SCardHelper.StringifyError(result));
            }

            SCardPCI recievePci = new SCardPCI();
            IntPtr sendPci = SCardPCI.GetPci(reader.ActiveProtocol);

            byte[] recieveBuffer = new byte[256];

            result = reader.Transmit(sendPci, apdu.ToArray(), recievePci, ref recieveBuffer);

            if (result != SCardError.Success)
            {
                context.Dispose();
                reader.Dispose();
                return string.Format("Cannot transmit data.{0} {1}", Environment.NewLine, SCardHelper.StringifyError(result));
            }

            var responseApdu = new ResponseApdu(recieveBuffer, IsoCase.Case2Short, reader.ActiveProtocol);

            message += string.Format("SW1: {1}{0}SW2: {2}{0}", Environment.NewLine, responseApdu.SW1, responseApdu.SW2);

            string data = responseApdu.HasData ? BitConverter.ToString(responseApdu.GetData()) : "--";

            if (uidOnly)
            {
                context.Dispose();
                reader.Dispose();
                return string.Format("UID^{0}^", data);
            }

            message += string.Format("UID: {0}",data);

            reader.EndTransaction(SCardReaderDisposition.Leave);
            reader.Disconnect(SCardReaderDisposition.Reset);

            context.Dispose();
            reader.Dispose();
            return message;
        }

        public void Disconnect()
        {
            if (_monitor == null)
                return;

            _monitor.Initialized -= (_cardInitalised);
            _monitor.CardInserted -= (_cardInserted);
            _monitor.CardRemoved -= (_cardRemoved);
            _monitor.Dispose();
        }
    }
}
