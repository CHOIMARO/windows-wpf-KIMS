using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.StaticAttribute.Enum {
    public enum deviceType {
        [StringValue("USB")]
        USB,
        [StringValue("SERIAL")]
        SERIAL,
    }
}
