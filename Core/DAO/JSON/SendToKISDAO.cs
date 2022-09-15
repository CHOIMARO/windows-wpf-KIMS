using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.DAO.JSON {
    public class SendToKISDAO {
        public string sig { get; set; }
        public int ver { get; set; }
        public string type { get; set; }
        public string cmd { get; set; }
        public dynamic msg { get; set; }
    }
}
