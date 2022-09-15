using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Ppose {
    public partial class pposeinfo {
        public int idx { get; set; }
        public DateTime timestamp { get; set; }
        public string ppose { get; set; }
        public string uninum { get; set; }
        public string rank { get; set; }
        public string uname { get; set; }
        public string stat { get; set; }
    }
}
