using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace External.TCP {
    public class TcpClientService : Service {
        System.Timers.Timer checkedTimer;
        string ip = string.Empty;
        int port = -1;
        //bool connectState = false;

        Ping ping = new Ping();
        byte[] bufferArray = new byte[32];

        public TcpClientService() {
            checkedTimer = new System.Timers.Timer();
            checkedTimer.Elapsed += CheckedTimer_Elapsed;
            checkedTimer.Interval = 1000;
        }

        private void CheckedTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            try {
                if (StaticAttribute.Function.tcpClient == null) {
                    return;
                }
                byte[] buff = Encoding.UTF8.GetBytes(".");
                StaticAttribute.Function.tcpClient.Send(buff, buff.Length, SocketFlags.None);
                Console.WriteLine(".");
                PingOptions pingOptions = new PingOptions();
                PingReply reply = ping.Send(IPAddress.Parse(StaticAttribute.Function.ip), 1, bufferArray, pingOptions);

                if(reply.Status == IPStatus.Success) {
                    Console.WriteLine("연결중");
                } else {
                    Console.WriteLine("연결 끊김");
                    throw new SocketException();
                }
                

            } catch (SocketException se) {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpClientServiceImpl.checkedConnectState.socketEx] " + se.Message);
                //socketException(se);
                StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                    stat = 0
                });
                Disconnect();
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpClientServiceImpl.checkedConnectState.Ex] " + ex.Message);
                //socketOtherException(ex);
            }
        }

        public bool Connect(string ip, int port) {
            this.ip = ip;
            this.port = port;
            StaticAttribute.Function.ip = ip;
            StaticAttribute.Function.port = port;
            //connectState = true;
            //checkedConnectThread = new Thread(checkedConnectState);
            //checkedConnectThread.Start();
            return Connect();
        }

        private bool Connect() {
            bool state = false;
            try {
                StaticAttribute.Function.tcpClient = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                //StaticAttribute.Function.tcpClient.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(connectCallBack), StaticAttribute.Function.tcpClient);
                var result = StaticAttribute.Function.tcpClient.BeginConnect(new IPEndPoint(IPAddress.Parse(ip), port), new AsyncCallback(connectCallBack), StaticAttribute.Function.tcpClient);

                bool success = result.AsyncWaitHandle.WaitOne(2000, true);
                if (success) {
                    if (StaticAttribute.Function.tcpClient != null) {
                        StaticAttribute.Function.tcpClient.EndConnect(result);
                    }
                    
                } else {
                    StaticAttribute.Function.tcpClient.Close();
                    throw new SocketException(10060);
                }

                state = true;
            } catch (SocketException se) {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpClientServiceImpl.connect] " + se.SocketErrorCode);
                StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                    stat = 2
                });
                Thread.Sleep(2000);
                if(StaticAttribute.Function.tcpClient != null) {
                    Connect();
                }else {
                    StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                        stat = 0
                    });
                }
                state = false;
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.TcpClientServiceImpl.connect] " + ex.Message);
                StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                    stat = 0
                });
                state = false;
            }
            return state;
        }

        private void connectCallBack(IAsyncResult ar) {
            try {
                // Retrieve the socket from the state object.  
                Socket client = (Socket)ar.AsyncState;
                client.Blocking = false;
                // Complete the connection.  
                //client.EndConnect(ar);
                
                if(client.Connected) {
                    Console.WriteLine("서버 연결 됨");
                    Console.WriteLine("Socket connected to {0}", client.RemoteEndPoint.ToString());
                    Console.WriteLine(string.Format("\r\nIP : {0}  //  PORT : {1}\r\n Connected tcp client", ip, port));
                    StaticAttribute.Function.logCommand.debugLog(string.Format("\r\nIP : {0}  //  PORT : {1}\r\n Connected tcp client", ip, port));
                    StaticAttribute.Function.tcpReceivedService.receivedHandler();
                    StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                        stat = 1
                    });
                    checkedTimer.Start();
                }
            } catch (Exception e) {
                Console.WriteLine(e.ToString());
                if (e.GetType().ToString().Equals("System.ObjectDisposedException")) {
                    Console.WriteLine("TCP 서버 연결 요청 취소");
                } else { //서버에서 응답 없음
                    if (StaticAttribute.Function.tcpClient != null) {
                        Connect();
                        StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new DAO.TcpIsConnectDAO {
                            stat = 2
                        });
                    }
                }
                StaticAttribute.Function.logCommand.errorLog("[External.TcpClientService." + e.Message + "]");
                StaticAttribute.Function.logCommand.debugLog("[External.TcpClientService." + e.Message + "]");
            }
        }
        private void socketException(SocketException se) {
            if (se.ErrorCode == 10061 || se.ErrorCode == 10060 || se.ErrorCode == 10057 || se.ErrorCode == 10053) {
                Disconnect();
                Connect();
            }
        }

        private void socketOtherException(Exception ex) {
            var w32ex = (System.ComponentModel.Win32Exception)ex.InnerException;
            if (w32ex != null) {
                var code = w32ex.ErrorCode;
                if (code == 10053 || code == 10054 || code == 10057) {
                    Console.WriteLine("서버 끊김");
                }
            }
            Console.WriteLine(ex);
        }
        public void SetObserver(dynamic info) {
            StaticAttribute.Function.tcpReceivedDataTracker.Subscribe(info);
        }

        public void SetIsConnectObserver(dynamic info) {
            StaticAttribute.Function.tcpIsConnectTracker.Subscribe(info);
        }

        public bool Disconnect() {
            bool state = false;
            try {
                if (StaticAttribute.Function.tcpClient != null) {
                    //StaticAttribute.Function.tcpClient.Shutdown(SocketShutdown.Both);
                    StaticAttribute.Function.tcpClient.Close();
                    StaticAttribute.Function.tcpClient.Dispose();

                    StaticAttribute.Function.tcpClient = null;
                    state = true;
                }
            } catch (Exception ex) {
            }

            checkedTimer.Stop();
            return state;
        }
    }
}
