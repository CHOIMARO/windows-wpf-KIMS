using KISM.DAO;
using KISM.StaticAttribute.Enum;
using KISM.View.AccountSetting;
using KISM.View.Function.Logout;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace KISM.ViewModel.AccountSetting {
    public class AccountSettingPageVM: INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #region DataContext
        private string loginTime = "10분 0초";
        public string LoginTime {
            get {
                return loginTime;
            }
            set {
                loginTime = value;
                onPropertyChanged("LoginTime");
            }
        }
        #endregion
        public void SetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.Subscribe(this);
            StaticAttribute.Function.tcpIsConnectTracker.Subscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Subscribe(info);
        }
        public void UnsetObserver(dynamic info) {
            StaticAttribute.Function.loginTimerTracker.UnSubscribe(this);
            StaticAttribute.Function.tcpIsConnectTracker.Unsubscribe(info);
            StaticAttribute.Function.tcpReceivedDataTracker2.Unsubscribe(info);
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }

        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        internal void Logout() {
            LogoutPage logoutPage = new LogoutPage();
            logoutPage.ShowDialog();

            if(StaticAttribute.Function.logoutPageState) {
                StaticAttribute.Function.logoutPageState = false;
                StaticAttribute.Function.logCommand.infoLog("[VM.AccountSettingPage.Log Out]");
                InsertLog(StaticAttribute.Enum.LogEnum.INFO, "계정 관리 페이지 로그아웃");
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();

            }
        }
    }
}
