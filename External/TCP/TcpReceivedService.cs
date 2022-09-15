using External.StaticAttribute;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.TCP {

    public class TcpReceivedService : Service {
        byte[] outbuff = new byte[1024];
        
        public void receivedHandler() {
            Function.tcpClient.BeginReceive(outbuff, 0, outbuff.Length, SocketFlags.None, new AsyncCallback(receivedCallBack), Function.tcpClient);
            
        }

        private async void receivedCallBack(IAsyncResult ar) {
            try {
                Socket client = (Socket)ar.AsyncState;
                int readSize = client.EndReceive(ar);
                if (readSize != 0) {
                    string data = Encoding.UTF8.GetString(outbuff, 0, readSize);
                    Console.WriteLine(data);
                    await Task.Run(() => Function.messageProcess.messageProcess(data));
                    receivedHandler();
                }
            } catch (SocketException se) { //LAN선 뽑히면 발생
                socketExceptionLogging("[External.TcpClientServiceImpl.receivedHandler] ", se);
                if (se.SocketErrorCode == SocketError.TimedOut || se.SocketErrorCode == SocketError.ConnectionReset || se.SocketErrorCode == SocketError.ConnectionAborted) {
                    reconnect();
                    StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                        stat = 0
                    });
                }
            } catch (Exception ex) {
                Console.WriteLine("[External.TcpReceivedService.receivedHandler] "+ex.GetType());
                if (ex.GetType().ToString().Equals("System.ObjectDisposedException")) {
                    Function.tcpClientService.Disconnect();
                    StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                        stat = 0
                    });
                    //reconnect();
                } else {
                    socketOtherExceptionLogging("[External.TcpClientServiceImpl.receivedHandler] ", ex);
                    reconnect();
                }
            }
        }

        private void reconnect() {
            Function.tcpClientService.Disconnect();
            Function.tcpClientService.Connect(Function.ip, Function.port);
        }

        private void socketExceptionLogging(string methodName, SocketException se) {
            Function.logCommand.errorLog(methodName + se.SocketErrorCode);
            Function.logCommand.debugLog(methodName + se.SocketErrorCode);
        }

        private void socketOtherExceptionLogging(string methodName, Exception ex) {
            Function.logCommand.errorLog(methodName + ex.Message);
            Function.logCommand.debugLog(methodName + ex.Message);
        }
    }
}
