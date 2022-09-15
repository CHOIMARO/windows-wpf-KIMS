using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.JSON.KIS100;
using KISM.DAO.JSON.KISM3;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.View.Function.Logout;
using KISM.View.SubPageDataGrid;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Threading;

namespace KISM {
    internal class MainWindowVM : IObserver<int>, IObserver<Core.DAO.JSON.ReceivedFromKISDAO>, IObserver<Core.DAO.TcpIsConnectDAO> {
        System.Timers.Timer loginTimer;
        int loginCount = 601;
        public void SetObserver() {
            StaticAttribute.Function.movePageTracker.Subscribe(this);

            //renewal
            StaticAttribute.Function.tcpConnectUseCase.SetIsConnectObserver(this);
            StaticAttribute.Function.tcpReceivedUseCase.SetObserver(this);
        }
        internal void GetSerialPort() {
            StaticAttribute.ConstAttribute.serPortList.AddRange(SerialPort.GetPortNames());
        }

        internal void CheckedDeviceConnect(int devType) {
            switch (devType) {
                case StaticAttribute.ConstAttribute.DBT_DEVTUP_PORT: //씨리얼 포트
                    //StaticAttribute.Function.connectPortUsecase.excute();
                    break;
                case StaticAttribute.ConstAttribute.DBT_DEVTUP_VOLUME: //USB
                    //StaticAttribute.Function.connectUsbUsecase.excute();
                    break;
            }
        }

        internal void CheckedDeviceDisconnect(int devType) {
            switch (devType) {
                case StaticAttribute.ConstAttribute.DBT_DEVTUP_PORT:
                    //StaticAttribute.Function.disconnectPortUsecase.excute();
                    break;
                case StaticAttribute.ConstAttribute.DBT_DEVTUP_VOLUME:
                    //StaticAttribute.Function.disconnectUsbUsecase.excute();
                    break;
            }
        }

        private string BytesToString(byte[] bytes) {
            return Encoding.Default.GetString(bytes).TrimEnd('\0');
        }
        // String을 바이트 배열로 변환
        private byte[] StringToBytes(string str) {
            return Encoding.UTF8.GetBytes(str);
        }
        public void LoginTimerStart() {
            loginTimer = new System.Timers.Timer();
            loginTimer.Interval = 1000;
            loginTimer.Elapsed += loginTimer_Elapsed;
            loginTimer.Start();
        }
        private void loginTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e) {
            loginCount--;
            string minutes = (loginCount / 60).ToString();
            string seconds = (loginCount % 60).ToString();
            string time = minutes + "분 " + seconds + "초";
            StaticAttribute.Function.loginTimerTracker.TrackLoginTimerNotify(new LoginTimerDAO {
                time = time,
            });

            if (time.Equals("1분 0초")) {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                    AutoLogoutPage autoLogoutPage = new AutoLogoutPage();
                    autoLogoutPage.ShowDialog();
                }));
            }else if (time.Equals("0분 0초")) {
                Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() => {
                    System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                    Application.Current.Shutdown();
                }));
            }
        }
        public void LoginTimerStop() {
            if (loginTimer != null) {
                loginTimer.Stop();
                loginTimer.Dispose();
            }
        }
        public void LoginTimeExtensionBtn_Click() {
            loginCount = 601;
        }
        public void SetLoginCount(int loginCount) {
            this.loginCount = loginCount;
        }

        public void OnNext(int value) {
            SetLoginCount(value);
            LoginTimerStop();
            LoginTimerStart();
        }

        public void OnNext(Core.DAO.JSON.ReceivedFromKISDAO value) {
            ReceivedFromKISDAO newMessage = new ReceivedFromKISDAO {
                sig = value.sig,
                ver = value.ver,
                type = (typeEnum)value.type,
                cmd = (commandEnum)value.cmd,
                msg = value.msg,
            };

            ClassifyMessage(newMessage);

            StaticAttribute.Function.tcpReceivedDataTracker2.TrackReceivedDataNotify(newMessage);
        }

        //MainPageVM에서 처리할 메세지들
        private void ClassifyMessage(ReceivedFromKISDAO newMessage) {
            switch (newMessage.type) {
                case typeEnum.REQ:
                    switch (newMessage.cmd) {
                        case commandEnum.HELLO:
                            InsertInjectorInfo(newMessage); //주입기 정보 DB에 입력
                            CurrentConnectedInjector(newMessage);
                            SendENDHelloMessage();
                            SendREQUckMessage();
                            break;
                    }
                    break;
                case typeEnum.RES:
                    switch (newMessage.cmd) {
                        case commandEnum.HELLO:
                            InsertInjectorInfo(newMessage); //주입기 정보 DB에 입력
                            CurrentConnectedInjector(newMessage);
                            SendENDHelloMessage();
                            SendREQUckMessage();
                            break;
                        case commandEnum.UCK:
                            if(newMessage.msg.stat.Equals("Y")) {
                                StaticAttribute.Function.accountState = true;
                            } else {
                                StaticAttribute.Function.accountState = false;
                            }
                            break;
                    }
                    break;
                case typeEnum.END:
                    break;
            }
        }

        private void CurrentConnectedInjector(ReceivedFromKISDAO newMessage) {
            StaticAttribute.Function.connectedInjectorDAO = new DAO.TCP.ConnectedInjectorDAO {
                id = newMessage.msg.id,
                hw = newMessage.msg.hw,
                name = newMessage.msg.name,
                fw = newMessage.msg.fw,
                sn = newMessage.msg.sn
            };
        }

        private void SendREQUckMessage() {
            Thread.Sleep(500);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.UCK, "");
            }));
        }

        private void InsertInjectorInfo(ReceivedFromKISDAO newMessage) {
            StaticAttribute.Function.insertInjectorInfoItemUseCase.Execute(new KIS100Hello {
                id = newMessage.msg.id,
                hw = newMessage.msg.hw,
                name = newMessage.msg.name,
                fw = newMessage.msg.fw,
                sn = newMessage.msg.sn
            });
        }

        //TCP 연결됬을 때, 또는 끊기거나 연결 중일 때 값 전달 
        // 연결 됨 : 1 //연결 끊김 : 0 //연결 중 : 2
        public void OnNext(Core.DAO.TcpIsConnectDAO value) {
            if (value.stat == 1) {
                StaticAttribute.Function.tcpConnect = 1;
                SendREQHelloMessage();
            } else if (value.stat == 2) {
                InitializeConnectInformation();
                StaticAttribute.Function.tcpConnect = 2;
                StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new TcpIsConnectDAO {
                    stat = 2
                });
                
            } else if (value.stat == 0) {
                InitializeConnectInformation();
                StaticAttribute.Function.tcpConnect = 0;
                StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new TcpIsConnectDAO {
                    stat = 0
                });
            }
        }

        private void InitializeConnectInformation() {
            StaticAttribute.Function.authState = false;
            StaticAttribute.Function.connectedInjectorDAO = new DAO.TCP.ConnectedInjectorDAO();
            StaticAttribute.Function.accountState = false;
        }

        private void SendREQHelloMessage() {
            Thread.Sleep(2000);
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(() => {
                string hashedMsg = StaticAttribute.Function.encryptionCommand.SHA256Hash("KISM3");
                StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.HELLO, new FromKISM3Hello {
                    id = hashedMsg.ToUpper(),
                });
            }));
        }
        private void SendENDHelloMessage() {
            StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.END, Core.StaticAttribute.Enum.commandEnum.HELLO, new FromKISM3HelloToo {
                id = StaticAttribute.Function.getMacAddress.getMacAddress()
            });

            //Connect Tracker Broadcast
            StaticAttribute.Function.authState = true;
            StaticAttribute.Function.tcpIsConnectTracker.TrackConnectNotify(new TcpIsConnectDAO {
                stat = 1
            });
            Console.WriteLine("상호 인증 완료");
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }
        
    }
}
