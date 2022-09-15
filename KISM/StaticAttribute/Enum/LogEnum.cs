using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.StaticAttribute.Enum {
    public enum LogEnum {
        [StringValue("INFO")]
        INFO,
        [StringValue("WARN")]
        WARN,
        [StringValue("ERROR")]
        ERROR,

    }
}
