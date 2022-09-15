using KISM.DAO;
using KISM.DAO.Account;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.AccountSetting;
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
    public class CreateAccountPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
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
        public void AddColumnData() {
            var accountDBList = StaticAttribute.Function.selectAccountInfoAllUseCase.Execute();
            foreach (var accountDB in accountDBList) {

                RegisteredDataRow.Add(new AccountInfoDAO {
                    Num = (RegisteredDataRow.Count+1),
                    Idx = accountDB.idx,
                    RegDate = accountDB.timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    UserId = accountDB.uid,
                    UniNum = accountDB.uninum.ToString(),
                    Department = accountDB.dpt.ToString(),
                    Rank = accountDB.rank,
                    Name = accountDB.uname,
                    Tel = accountDB.tel,
                    Email = accountDB.email,
                    Stat = accountDB.stat.Equals("A") ? "활성화" : accountDB.stat.Equals("P") ? "비활성화" : "삭제"
                });
            }
        }

        public void ShowRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                RegisteredDataRow.Clear();
                AddColumnData();
            }));

        }

        internal bool InsertAccount(string password, string rank, int auth) {
            string processingPw = StaticAttribute.Function.encryptionCommand.dataHashing(IDTxt, password);
            var info = SetAccountInfo(processingPw, rank, auth);
            bool state = StaticAttribute.Function.InsertAccountInfoItemUseCase.Execute(info);

            if (state) {
                InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.insertAccountInfoSuccess);
                ShowRegisteredData();
            } else {
                InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.insertAccountInfoFailed);
            }
            return state;
            // TODO : state == true ? account info field init
        }

        private accountInfo SetAccountInfo(string processingPw, string rank, int auth) {
            return new accountInfo {
                timestamp = DateTime.Now,
                uid = IDTxt,
                upw = processingPw,
                dpt = Department,
                uninum = UniqueNumber,
                rank = rank.Trim(),
                uname = UserName,
                email = Email,
                tel = Tel,
                pmit = auth == 0 ? 100 : auth == 1 ? 200 : 300,
                stat = "A"
            };
        }

        public bool CheckAccountStat() {
            bool state = true;
            foreach(var data in RegisteredDataRow) {
                if(data.Stat.Equals("활성화")) {
                    state = false;
                    break;
                }
            }
            return state;
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
