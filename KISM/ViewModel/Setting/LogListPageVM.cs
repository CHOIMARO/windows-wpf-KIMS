using KISM.DAO.Log;
using KISM.DAO;
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

namespace KISM.ViewModel.Setting {
    public class LogListPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
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
        #endregion

        internal void Init() {
            
        }
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
        public async Task<bool> ShowRegisteredData() {
            RegisteredDataRow.Clear();
            await Task.Delay(100);
            AddColumnData();
            return true;
        }
        private void AddColumnData() {
            var logInfoAll = StaticAttribute.Function.selectLogInfoAllUseCase.Execute();
            foreach (var data in logInfoAll) {
                RegisteredDataRow.Add(new loginfoDAO {
                    Idx = data.idx.ToString(),
                    Type = data.type,
                    Message = data.message,
                    Timestamp = data.timestamp.ToString(),
                    Num = (RegisteredDataRow.Count + 1).ToString()
                });
            }
        }

        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }

        public void CheckUserSearch(string datePickerStart, string datePickerEnd, string msgStat) {
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

            List<loginfo> selectLogInfoList = SelectLogInfoData(datePickerSt, datePickerE, msgStatus);
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
        public List<loginfo> SelectLogInfoData(DateTime startDate, DateTime endDate, string msgStat) {
            List<loginfo> logInfoAll = StaticAttribute.Function.processingDBData.ProcessingLogInfoAll(StaticAttribute.Function.selectLogInfoAllUseCase.Execute());
            List<loginfo> processedDtInfoList = new List<loginfo>();
            foreach (var logInfoData in logInfoAll) {
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

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        public void OnNext(LoginTimerDAO value) {
            LoginTime = value.time;
        }
    }
}
