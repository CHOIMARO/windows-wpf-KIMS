using KISM.DAO;
using KISM.DAO.Log;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.SubPageVM {
    public class LogListPageVM : INotifyPropertyChanged, IObserver<TcpCmdTrackerResDAO>, IObserver<RlogMsgDAO>, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        List<loginfo> selectLogInfoFromDBList = new List<loginfo>();

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

        private Visibility logRegRequestWindow = Visibility.Collapsed;

        public Visibility LogRegRequestWindow {
            get {
                return logRegRequestWindow;
            }
            set {
                logRegRequestWindow = value;
                onPropertyChanged("LogRegRequestWindow");
            }
        }

        private ObservableCollection<string> typeItems = new ObservableCollection<string> {
            "전체 선택","INFO" ,"WARN", "ERROR",
        };
        public ObservableCollection<string> TypeItems {
            get {
                return typeItems;
            }
            set {
                typeItems = value;
                onPropertyChanged("TypeItems");
            }
        }
        private string todayDate = DateTime.Now.ToString("yyyy-MM-dd");
        public string TodayDate {
            get {
                return todayDate;
            }
            set {
                todayDate = value;
                onPropertyChanged("TodayDate");
            }
        }
        private ObservableCollection<loginfoDAO> registeredDataRow = new ObservableCollection<loginfoDAO>();
        public ObservableCollection<loginfoDAO> RegisteredDataRow {
            get {
                return registeredDataRow;
            }
            set {
                registeredDataRow = value;
                onPropertyChanged("RegisteredDataRow");
            }
        }
        private Visibility createKeyRequestWindow = Visibility.Collapsed;

        public Visibility CreateKeyRequestWindow {
            get {
                return createKeyRequestWindow;
            }
            set {
                createKeyRequestWindow = value;
                onPropertyChanged("CreateKeyRequestWindow");
            }
        }
        #endregion
        int pmit = -1;

        internal void init() {
            pmit = StaticAttribute.ConstAttribute.userInfo.pmit;
        }
        public void setObserver(dynamic info) {
            StaticAttribute.Function.tcpCmdTrackerResUsecase.excute(this);
            StaticAttribute.Function.rlogMsgTracker.Subscribe(this);

            StaticAttribute.Function.loginExtensionTracker.Subscribe(info);
            StaticAttribute.Function.loginTimerTracker.Subscribe(this);
        }
        public void endTransmission() {
            StaticAttribute.Function.tcpCmdTrackerResUsecase.endTransmission();
            //StaticAttribute.Function.rlogMsgTracker.EndTransmission();
        }
        public async Task<bool> loadingList() {
            
            selectLogInfoFromDBList.Clear();
            RegisteredDataRow.Clear();
            selectLogInfoFromDBList = StaticAttribute.Function.selectLogInfoUsecase.excute();

            await Task.Delay(100);
            foreach (var data in selectLogInfoFromDBList) {
                RegisteredDataRow.Add(new loginfoDAO {
                    Idx = data.idx.ToString(),
                    Type = data.type,
                    Message = data.message,
                    Timestamp = data.timestamp.ToString(),
                    Num = (RegisteredDataRow.Count + 1).ToString()
                });
            }
            return true;
            
        }
        public bool insertLog(LogEnum logEnum, string message) {
            return StaticAttribute.Function.insertLogInfoUsecase.excute(logEnum, message);
        }

        public void checkUserSearch(string datePickerStart, string datePickerEnd, string msgStat) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                RegisteredDataRow.Clear();
            }));
            DateTime datePickerSt = DateTime.Now.Date;
            DateTime datePickerE = DateTime.Now.Date;
            string msgStatus = string.Empty;
            
            if (msgStat.Equals("전체 선택")) {
                msgStatus = "";
            } else {
                msgStatus = msgStat;
            }

            if (datePickerStart.Length == 0) {
                datePickerSt = DateTime.Now.Date;
            } else {
                datePickerSt = Convert.ToDateTime(datePickerStart);
            }
            if (datePickerEnd.Length == 0) {
                datePickerE = DateTime.Now.Date.AddDays(1);
            } else {
                datePickerE = Convert.ToDateTime(datePickerEnd);
            }

            List<loginfo> selectLogInfoList = selectLogInfoData(datePickerSt, datePickerE, msgStatus);
            foreach (var data in selectLogInfoList) {
                RegisteredDataRow.Add(new loginfoDAO {
                    Idx = data.idx.ToString(),
                    Type = data.type,
                    Message = data.message,
                    Timestamp = data.timestamp.ToString(),
                    Num = (RegisteredDataRow.Count + 1).ToString()
                });
            }

        }
        public List<loginfo> selectLogInfoData(DateTime startDate, DateTime endDate, string msgStat) {
            List<loginfo> logInfoList = StaticAttribute.Function.selectLogInfoUsecase.excute();
            List<loginfo> processedDtInfoList = new List<loginfo>();
            foreach (var logInfoData in logInfoList) {
                if (logInfoData.timestamp >= startDate && logInfoData.timestamp <= endDate) {
                        if (msgStat.ToString().Length == 0) {
                            processedDtInfoList.Add(logInfoData);
                        } else if (msgStat.ToString().Equals("INFO") && logInfoData.type.Equals("INFO")) {
                            processedDtInfoList.Add(logInfoData);
                        } else if (msgStat.ToString().Equals("WARN") && logInfoData.type.Equals("WARN")) {
                            processedDtInfoList.Add(logInfoData);
                        } else if (msgStat.ToString().Equals("ERROR") && logInfoData.type.Equals("ERROR")) {
                            processedDtInfoList.Add(logInfoData);
                        }
                }
            }
            return processedDtInfoList;
        }

        public void OnNext(TcpCmdTrackerResDAO value) {
            if (StaticAttribute.Function.createKeyRequestWindow) {
                CreateKeyRequestWindow = pmit == 100 || pmit == 200 ? Visibility.Visible : Visibility.Hidden;
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }
        public void closeCreateKeyRequestWindow() {
            CreateKeyRequestWindow = Visibility.Collapsed;
        }
        public void showLogRegRequestWindow() {
            LogRegRequestWindow = Visibility.Visible;
        }
        public void closeLogRegRequestWindow() {
            LogRegRequestWindow = Visibility.Collapsed;
        }
        public void OnNext(RlogMsgDAO value) {
            if (value.state) {
                LogRegRequestWindow = Visibility.Visible;
            }
        }

        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }
    }
}
