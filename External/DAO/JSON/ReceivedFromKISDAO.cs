using External.StaticAttribute.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.DAO.JSON {
    public class ReceivedFromKISDAO {
        public string sig { get; set; }
        public int ver { get; set; }
        public typeEnum type { get; set; }
        public commandEnum cmd { get; set; }
        public dynamic msg { get; set; }
    }
}
