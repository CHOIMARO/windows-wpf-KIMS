using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.JSON {
    public class KISMDataDAO {
        public string sig { get; set; }
        public int ver { get; set; }
        public string type { get; set; }
        public string cmd { get; set; }
        public dynamic msg { get; set; }
    }
}
