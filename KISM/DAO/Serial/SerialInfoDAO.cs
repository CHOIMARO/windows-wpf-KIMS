using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Serial {
    public class SerialInfoDAO {
        public string timeStamp { get; set; }
        public string port { get; set; }
        public string baudRate { get; set; }
        public string parity { get; set; }
        public string dataBit { get; set; }
        public string stopBit { get; set; }
    }
}
