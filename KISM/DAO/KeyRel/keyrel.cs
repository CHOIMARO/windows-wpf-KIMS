using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.KeyRel {
    public partial class keyrel {
        public int idx { get; set; }
        public DateTime timestamp { get; set; }
        public int ijidx { get; set; }
        public string dpt { get; set; }
        public string ppose { get; set; }
        public DateTime stdate { get; set; }
        public DateTime expdate { get; set; }
        public DateTime? dtdate { get; set; }
        public DateTime? deldate { get; set; }
        public string wkey { get; set; }
        public string stat { get; set; }
    }
}
