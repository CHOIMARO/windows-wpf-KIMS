using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Serial {
    public class serialinfo {
        public int idx { get; set; }
        public string timestamp { get; set; }
        public string port { get; set; }
        public string baud_rate { get; set; }
        public string parity { get; set; }
        public string data_bit { get; set; }
        public string stop_bit { get; set; }
    }
}
