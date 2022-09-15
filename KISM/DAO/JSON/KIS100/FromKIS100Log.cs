using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.JSON.KIS100 {
    public class FromKIS100Log {
        public string name { get; set; }
        public DateTime timestamp { get; set; }
        public string ip { get; set; }
        public int stat { get; set; }
        public string hw { get; set; }
        public string sn { get; set; }
        //public string ppose { get; set; }
        public string ppo { get; set; }
        public string grp { get; set; }
    }
}
