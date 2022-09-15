using External.DAO.JSON;
using External.DAO.JSON.KISM3;
using External.StaticAttribute.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.TCP {
    public class TcpSendService : Service {
        public bool Send(string json) {
            try {
                if (StaticAttribute.Function.tcpClient != null) {
                    byte[] buff = Encoding.UTF8.GetBytes(json);
                    StaticAttribute.Function.tcpClient.BeginSend(buff, 0, buff.Length,
                        SocketFlags.None, new AsyncCallback(sendCallBack), StaticAttribute.Function.tcpClient);
                    Console.WriteLine(json);
                } else {
                    return false;
                }
                
                return true;
            } catch (NullReferenceException ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpSendService.send] " + ex.Message);
                return false;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpSendService.send] " + ex.Message);
                return false;
            }
        }

        private void sendCallBack(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;
                // Complete sending the data to the remote device.  
                int bytesSent = client.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to server.", bytesSent);

            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                StaticAttribute.Function.logCommand.errorLog("[External.TcpSendService." + e.Message + "]");
            }
        }
    }
}
