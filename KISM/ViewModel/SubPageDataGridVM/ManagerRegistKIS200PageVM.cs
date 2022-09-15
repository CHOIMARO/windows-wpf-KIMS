using KISM.DAO.JSON.KISM3;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Loading;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.SubPageDataGridVM {
    class ManagerRegistKIS200PageVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #region DataContext
        private double userIconOpacity = 0.6;
        public double UserIconOpacity {
            get {
                return userIconOpacity;
            }
            set {
                userIconOpacity = value;
                onPropertyChanged("UserIconOpacity");
            }
        }

        private System.Windows.Media.Brush userIconForeground = System.Windows.Media.Brushes.White;
        public System.Windows.Media.Brush UserIconForeground {
            get { return userIconForeground; }
            set {
                userIconForeground = value;
                onPropertyChanged("UserIconForeground");
            }
        }

        private System.Windows.Media.Brush userIconBackground = System.Windows.Media.Brushes.Black;
        public System.Windows.Media.Brush UserIconBackground {
            get { return userIconBackground; }
            set {
                userIconBackground = value;
                onPropertyChanged("UserIconBackground");
            }
        }
        #endregion

        public void SetObserver(dynamic info) {
            StaticAttribute.Function.tcpIsConnectTracker.Subscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Subscribe(info);
        }
        public void UnsetObserver(dynamic info) {
            StaticAttribute.Function.tcpIsConnectTracker.Unsubscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Unsubscribe(info);
        }
        public void RegistUserReq(string id, string pwd) {
            StaticAttribute.Function.injectorID = id;
            StaticAttribute.Function.injectorPW = pwd;
            string sendPwd = StaticAttribute.Function.encryptionCommand.dataHashing(id, pwd);

            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                StaticAttribute.Function.loadingMessage.setMessage("주입기 계정 설정 중..");
            }));


            if (StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200") && StaticAttribute.Function.serialConnect) {
                
            }else if(StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200") && StaticAttribute.Function.tcpConnect == 1) {
                StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.UREG, new FromKISM3UserReg {
                    uid = id,
                    upw = sendPwd
                });
            }

            StaticAttribute.Function.logCommand.debugLog("[VM.ManagerRegistrationPage.Request User Regist]");
            InsertLog(LogEnum.INFO, "주입기로 주입기 계정 설정 메세지 송신.");
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }
        public void CheckUser() {
            if (StaticAttribute.Function.accountState) {
                UserIconForeground = System.Windows.Media.Brushes.Green;
                UserIconBackground = System.Windows.Media.Brushes.White;
                UserIconOpacity = 1.0;
            } else {
                UserIconForeground = System.Windows.Media.Brushes.White;
                UserIconBackground = System.Windows.Media.Brushes.Black;
                UserIconOpacity = 0.6;
            }
        }

        public void OnNext(RegistUserStatusDAO value) {
            if (value.status.Equals("Y")) {
                StaticAttribute.Function.accountState = true;
                CheckUser();
                InformationMessage.InformationShowDialog("ID/PW 입력이 완료되었습니다.");
            }
        }
        public void OnNext(UDelDAO value) {
            if (value.stat.ToString().Equals("Y")) {
                StaticAttribute.Function.accountState = false;
                CheckUser();
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }
    }
}
