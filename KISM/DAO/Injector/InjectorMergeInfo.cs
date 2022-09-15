using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.Injector {
    public class InjectorMergeInfo {
        public DateTime timestamp { get; set; }
        public string ijid { get; set; }
        public int ijidx { get; set; }
        public string uid { get; set; }
        public string hw { get; set; }
        public string fw { get; set; }
        public string sn { get; set; }
        public string stat { get; set; }
    }
}
