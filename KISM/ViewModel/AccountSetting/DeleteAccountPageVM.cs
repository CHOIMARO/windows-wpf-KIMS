using KISM.DAO;
using KISM.DAO.Account;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.AccountSetting {
    class DeleteAccountPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        #region DataContext

        private Visibility deleteAccountWindow = Visibility.Collapsed;

        public Visibility DeleteAccountWindow {
            get {
                return deleteAccountWindow;
            }
            set {
                deleteAccountWindow = value;
                onPropertyChanged("DeleteAccountWindow");
            }
        }
        private ObservableCollection<AccountInfoDAO> registeredDataRow = new ObservableCollection<AccountInfoDAO>();
        public ObservableCollection<AccountInfoDAO> RegisteredDataRow {
            get {
                return registeredDataRow;
            }
            set {
                registeredDataRow = value;
                onPropertyChanged("RegisteredDataRow");
            }
        }
        private ObservableCollection<string> authItems = new ObservableCollection<string> {
            "마스터 계정", "관리자 계정", "일반 계정"
        };

        public ObservableCollection<string> AuthItems {
            get {
                return authItems;
            }
            set {
                authItems = value;
                onPropertyChanged("AuthItems");
            }
        }

        private ObservableCollection<string> rankItems = new ObservableCollection<string> {
            "이병", "일병", "상병", "병장", "하사", "중사", "상사","원사","준위",
            "소위", "중위", "대위", "소령", "중령","대령","준장", "소장", "중장", "대장", "원수"

        };

        public ObservableCollection<string> RankItems {
            get {
                return rankItems;
            }
            set {
                rankItems = value;
                onPropertyChanged("RankItems");
            }
        }

        private string idTxt = string.Empty;
        public string IDTxt {
            get {
                return idTxt;
            }
            set {
                idTxt = value;
                onPropertyChanged("IDTxt");
            }
        }

        private string uniqueNumber = string.Empty;
        public string UniqueNumber {
            get {
                return uniqueNumber;
            }
            set {
                uniqueNumber = value;
                onPropertyChanged("UniqueNumber");
            }
        }

        private string department = string.Empty;
        public string Department {
            get {
                return department;
            }
            set {
                department = value;
                onPropertyChanged("Department");
            }
        }

        private string userName = string.Empty;
        public string UserName {
            get {
                return userName;
            }
            set {
                userName = value;
                onPropertyChanged("UserName");
            }
        }

        private string email = string.Empty;
        public string Email {
            get {
                return email;
            }
            set {
                email = value;
                onPropertyChanged("Email");
            }
        }

        private string tel = string.Empty;
        public string Tel {
            get {
                return tel;
            }
            set {
                tel = value;
                onPropertyChanged("Tel");
            }
        }
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

        accountInfo selAccountInfo = new accountInfo();

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
        public void AddColumnData() {
            var accountDBList = StaticAttribute.Function.selectAccountInfoAllUseCase.Execute();
            foreach (var accountDB in accountDBList) {

                if(accountDB.stat.Equals("A")) {
                    RegisteredDataRow.Add(new AccountInfoDAO {
                        Num = (RegisteredDataRow.Count + 1),
                        Idx = accountDB.idx,
                        RegDate = accountDB.timestamp.ToString(),
                        UserId = accountDB.uid,
                        UniNum = accountDB.uninum.ToString(),
                        Department = accountDB.dpt.ToString(),
                        Rank = accountDB.rank,
                        Name = accountDB.uname,
                        Stat = accountDB.stat.Equals("A") ? "활성화" : accountDB.stat.Equals("P") ? "비활성화" : "삭제",
                        Tel = accountDB.tel,
                        Email = accountDB.email
                    });
                }
            }
        }

        public void ShowRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                registeredDataRow.Clear();
                AddColumnData();
            }));

        }

        internal bool DeleteAccountInfoItem(int idx) {
            bool state = StaticAttribute.Function.deleteAccountInfoItemUseCase.Execute(idx);

            if (state) {
                StaticAttribute.Function.logCommand.infoLog("[VM.DeleteAccountPage.Delete Account Success]");
                InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.deleteAccountInfoSuccess);
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            } else {
                StaticAttribute.Function.logCommand.infoLog("[VM.DeleteAccountPage.Delete Account Failed]");
                InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.deleteAccountInfoFailed);
            }
            return state;
            //TODO: state == true ? account info field init
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
    }
}
