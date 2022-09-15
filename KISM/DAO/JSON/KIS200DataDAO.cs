using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.JSON {
    public class KIS200DataDAO {
        public string sig { get; set; }
        public int ver { get; set; }
        //public typeEnum type { get; set; }
        //public commandEnum cmd { get; set; }
        public string type { get; set; }
        public string cmd { get; set; }
        public dynamic msg { get; set; }
    }
}
