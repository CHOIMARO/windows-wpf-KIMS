using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.StaticAttribute.Enum {
    public enum commandEnum {
        [StringValue("HELLO")]
        HELLO,
        [StringValue("UCK")]
        UCK,
        [StringValue("UREG")]
        UREG,
        [StringValue("GEN")]
        GEN,
        [StringValue("KREG")]
        KREG,
        [StringValue("LOG")]
        LOG,
        [StringValue("DLOG")]
        DLOG,
        [StringValue("REK")]
        REK,
        [StringValue("KDEL")]
        KDEL,
        [StringValue("UDEL")]
        UDEL,
        [StringValue("RLOG")]
        RLOG,
    }

    public enum typeEnum {
        [StringValue("REQ")]
        REQ,
        [StringValue("RES")]
        RES,
        [StringValue("END")]
        END,
    }
}
