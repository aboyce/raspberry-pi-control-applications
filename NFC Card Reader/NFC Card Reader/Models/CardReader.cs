using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PCSC;

namespace NFC_Card_Reader.Models
{
    public class CardReader
    {
        private SCardContext _context;
        private SCardReader _reader;

        public string Name { get; set;}


        public CardReader() : this("Unknown") { }

         public CardReader(string name)
        {
            _context = new SCardContext();
            _reader = new SCardReader(_context);
            Name = name;
        }
 

        public bool Establish()
        {
            try
            {
                _context.Establish(SCardScope.System);
            }
            catch (Exception)
            {
                return false;
            }

            return true;
        }

        public string[] GetReaders()
        {
            return _context.GetReaders();
        }

        public void CleanUp()
        {
            //_reader.Disconnect;
            _reader.Dispose();
            _context.Dispose();
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
