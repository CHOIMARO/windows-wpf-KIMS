using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Account {
    public class accountInfo {
        public int idx { get; set; }
        public DateTime timestamp { get; set; }
        public string uid { get; set; }
        public string upw { get; set; }
        public string uname { get; set; }
        public string email { get; set; }
        public string tel { get; set; }
        public string uninum { get; set; }
        public string dpt { get; set; }
        public string rank { get; set; }
        public int pmit { get; set; }
        public string stat { get; set; }
        public string note { get; set; }
    }
}
