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

        public string Name { get { return _reader.ReaderName; } }


        
        public CardReader(SCardContext context)
        {
            _reader = new SCardReader(context);
        }


        public void Establish()
        {
            _context.Establish(SCardScope.System);
        }
    }
}
