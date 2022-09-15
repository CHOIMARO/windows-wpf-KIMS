using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.StaticAttribute.Enum {
    public enum ServerCommandEnum {
        [StringValue("HELLO")]
        HELLO,
        [StringValue("LOGIN")]
        LOGIN,
        [StringValue("GETLOGIN")]
        GETLOGIN,
        [StringValue("ACCOUNT")]
        ACCOUNT,
        [StringValue("RMOD")]
        RMOD,
        [StringValue("UMOD")]
        UMOD,
        [StringValue("DMOD")]
        DMOD,
        [StringValue("GKEY")]
        GKEY,
        [StringValue("RKEY")]
        RKEY,
        [StringValue("DKEY")]
        DKEY,
        [StringValue("RUSER")]
        RUSER,
        [StringValue("UUSER")]
        UUSER,
        [StringValue("DUSER")]
        DUSER,
        [StringValue("RGRP")]
        RGRP,
        [StringValue("UGRP")]
        UGRP,
        [StringValue("DGRP")]
        DGRP,
        [StringValue("RPPO")]
        RPPO,
        [StringValue("UPPO")]
        UPPO,
        [StringValue("DPPO")]
        DPPO,
        [StringValue("RLOG")]
        RLOG,
        [StringValue("ADD")]
        ADD,
        [StringValue("UPDATE")]
        UPDATE,
        [StringValue("DELETE")]
        DELETE,

        [StringValue("KREQ")]
        KREQ,
        [StringValue("ULOG")]
        ULOG,

        [StringValue("LIST")]
        LIST,
        [StringValue("GET")]
        GET,
    }
}
