using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace KISM.Util {
    public class GetMacAddress {
        public string getMacAddress() {
            return StaticAttribute.Function.encryptionCommand.SHA256Hash(NetworkInterface.GetAllNetworkInterfaces()[0].GetPhysicalAddress().ToString());
        }
    }
}
