using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Comm {
    public partial class comminfo {
        public int idx { get; set; }
        public DateTime timestamp { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
    }
}
