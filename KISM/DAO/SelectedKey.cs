using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO {
    public class SelectedKey {
        public int num { get; set; }
        public int idx { get; set; }
        public DateTime timestamp { get; set; }
        public DateTime stDate { get; set; }
        public DateTime expDate { get; set;}
        public string dpt { get; set; }
        public string ppose { get; set; }
        public string wkey { get; set; }
        public string stat { get; set; }
    }
}
