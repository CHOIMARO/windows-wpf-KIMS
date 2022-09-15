using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.Util {
    public class HexAndByte {
        public byte[] hexToByte(string convertString) {
            byte[] convertArr = new byte[convertString.Length / 2];

            for (int i = 0; i < convertArr.Length; i++) {
                convertArr[i] = Convert.ToByte(convertString.Substring(i * 2, 2), 16);
            }
            return convertArr;
        }

        public string byteToHex(byte[] bytes) {
            string hex = BitConverter.ToString(bytes);
            return hex.Replace("-", "");
        }

        public byte[] StringToBytes(string str) {
            return Encoding.UTF8.GetBytes(str);
        }

        public string BytesToString(byte[] bytes) {
            return Encoding.Default.GetString(bytes).TrimEnd('\0');
        }
    }
}
