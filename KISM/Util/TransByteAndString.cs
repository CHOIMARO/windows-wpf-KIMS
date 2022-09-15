using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.Util {
    public class TransByteAndString {

        // 바이트 배열을 String으로 변환
        public string BytesToString(byte[] bytes) {
            return Encoding.Default.GetString(bytes).TrimEnd('\0');
        }
        // String을 바이트 배열로 변환
        public byte[] StringToBytes(string str) {
            return Encoding.UTF8.GetBytes(str);
        }
    }
}
