using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.StaticAttribute.Enum {
    public enum SerialInfoEnum {
        [StringValue("serial_info")]
        TABLENAME,
        [StringValue("time_stamp")]
        TIMESTAMP,
        [StringValue("port")]
        PORT,
        [StringValue("baud_rate")]
        BAUDRATE,
        [StringValue("parity")]
        PARITY,
        [StringValue("data_bit")]
        DATABIT,
        [StringValue("stop_bit")]
        STOPBIT
    }

    public enum ParityInfoEnum {
        [StringValue("None")]
        NONE,
        [StringValue("Odd")]
        ODD,
        [StringValue("Even")]
        EVEN,
        [StringValue("Mark")]
        MARK,
        [StringValue("Space")]
        SPACE
    }

    public enum StopBitsInfoEnum {
        [StringValue("0")]
        NONE,
        [StringValue("1")]
        ONE,
        [StringValue("1.5")]
        OnePointFive,
        [StringValue("2")]
        TWO,
    }
}
