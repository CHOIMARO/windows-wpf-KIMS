using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Distribution {
    public partial class dtinfo {
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        public int ijidx { get; set; }

        public string mdid { get; set; }

        public string logid { get; set; }

        public string mdip { get; set; }
        public string hw { get; set; }
        public string sn { get; set; }
        public string dpt { get; set; }
        public string ppose { get; set; }

        public string stat { get; set; }

    }
}
