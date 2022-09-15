using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.StaticAttribute.Enum {
    public enum collectionEnum {
        [StringValue("auth")]
        AUTH,
        [StringValue("account")]
        ACCOUNT,
        [StringValue("device")]
        DEVICE,
        [StringValue("skey")]
        SKEY,
        [StringValue("log")]
        LOG,
        [StringValue("setting/grp")]
        SETTINGGRP,
        [StringValue("setting/ppo")]
        SETTINGPPO,
    }
}
