using KISM.DAO;
using KISM.DAO.Distribution;
using KISM.DAO.KeyRel;
using KISM.DAO.Log;
using KISM.DAO.MilUnit;
using KISM.DAO.Ppose;
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

namespace KISM.ViewModel.SubPageDataGridVM {
    public class KeyDistributionRecordPageVM :INotifyPropertyChanged, IObserver<LoginTimerDAO> {
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

        private ObservableCollection<string> statItems = new ObservableCollection<string> {
            "전체 선택","키 배포 대기" ,"키 배포 완료", "키 재배포 완료", "키 삭제", "IP 갱신", "알 수 없음"
        };
        public ObservableCollection<string> StatItems {
            get {
                return statItems;
            }
            set {
                statItems = value;
                onPropertyChanged("StatItems");
            }
        }
        private ObservableCollection<string> mdItems = new ObservableCollection<string> {

        };
        public ObservableCollection<string> MdItems {
            get {
                return mdItems;
            }
            set {
                mdItems = value;
                onPropertyChanged("MdItems");
            }
        }
        private ObservableCollection<string> grpItems = new ObservableCollection<string> {
        };
        public ObservableCollection<string> GrpItems {
            get {
                return grpItems;
            }
            set {
                grpItems = value;
                onPropertyChanged("GrpItems");
            }
        }
        private ObservableCollection<string> pposeItems = new ObservableCollection<string> {

        };
        public ObservableCollection<string> PposeItems {
            get {
                return pposeItems;
            }
            set {
                pposeItems = value;
                onPropertyChanged("PposeItems");
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
        private ObservableCollection<DtInfoDAO> registeredDataRow = new ObservableCollection<DtInfoDAO>();
        public ObservableCollection<DtInfoDAO> RegisteredDataRow {
            get {
                return registeredDataRow;
            }
            set {
                registeredDataRow = value;
                onPropertyChanged("RegisteredDataRow");
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
        public bool ShowRegisteredData() {
            RegisteredDataRow.Clear();
            //await Task.Delay(100);
            AddColumnData();
            return true;
        }
        private void AddColumnData() {
            var dtInfoAll = StaticAttribute.Function.selectDtInfoAllUseCase.Execute();
            foreach (var dtInfo in dtInfoAll) {
                string state = dtInfo.stat.Equals("0") ? "키 배포 대기" :
                              dtInfo.stat.Equals("1") ? "키 배포 완료" :
                              dtInfo.stat.Equals("2") ? "키 재배포 완료" :
                              dtInfo.stat.Equals("3") ? "키 삭제" :
                              dtInfo.stat.Equals("4") ? "IP 갱신" : "알 수없음";

                RegisteredDataRow.Add(new DtInfoDAO {
                    Num = (RegisteredDataRow.Count+1).ToString(),
                    RegDate = dtInfo.timestamp.ToString(),
                    DvName = dtInfo.hw,
                    //Hw = dtInfo.hw + "/" + dtInfo.sn,
                    Sn = dtInfo.sn,
                    Dpt = dtInfo.dpt.ToString(),
                    Ppose = dtInfo.ppose,
                    Stat = state,
                });
            }
        }
        public void SetMdItems() {
            MdItems.Add("전체 선택");
            var dtInfoAll = StaticAttribute.Function.selectDtInfoAllUseCase.Execute();
            foreach (var dtInfoData in dtInfoAll) {
                if (!MdItems.Contains(dtInfoData.mdid)) {
                    MdItems.Add(dtInfoData.mdid);
                }
            }
        }
        public void SetGrp() {
            var milUnitAll = StaticAttribute.Function.selectMilUnitInfoAllUseCase.Execute();
            GrpItems.Add("전체 선택");
            foreach (var milUnitInfo in milUnitAll) {
                if (milUnitInfo.stat.Equals("A")) {
                    GrpItems.Add(milUnitInfo.unit);
                }
            }

        }
        public void SetPpose() {
            var pposeInfoAll = StaticAttribute.Function.selectPposeInfoAllUseCase.Execute();
            PposeItems.Add("전체 선택");
            foreach (var pposeInfo in pposeInfoAll) {
                if (pposeInfo.stat.Equals("A")) {
                    PposeItems.Add(pposeInfo.ppose);
                }
            }
        }

        public void CheckUserSearch(string datePickerStart, string datePickerEnd, string dvName, string keyStat) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                registeredDataRow.Clear();
            }));
            DateTime datePickerSt = DateTime.Now.Date;
            DateTime datePickerE = DateTime.Now.Date;
            string deviceName = string.Empty;
            string keyStatus = string.Empty;
            if (dvName.Equals("전체 선택")) {
                deviceName = "";
            } else {
                deviceName = dvName;
            }
            if (keyStat.Equals("전체 선택")) {
                keyStatus = "";
            } else {
                keyStatus = keyStat;
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

            List<dtinfo> selectDtInfoList = SelectKeyRelData(datePickerSt, datePickerE, deviceName, keyStatus);
            int count = 0;
            foreach (var dtInfo in selectDtInfoList) {
                string state = dtInfo.stat.Equals("0") ? "키 배포 대기" :
                              dtInfo.stat.Equals("1") ? "키 배포 완료" :
                              dtInfo.stat.Equals("2") ? "키 재배포 완료" :
                              dtInfo.stat.Equals("3") ? "키 삭제" :
                              dtInfo.stat.Equals("4") ? "IP 갱신" : "알 수없음";

                RegisteredDataRow.Add(new DtInfoDAO {
                    Num = count.ToString(),
                    RegDate = dtInfo.timestamp.ToString(),
                    DvName = dtInfo.mdid,
                    Hw = dtInfo.hw + "/" + dtInfo.sn,
                    //Sn = dtInfo.sn,
                    Dpt = dtInfo.dpt.ToString(),
                    Ppose = dtInfo.ppose,
                    Stat = state,

                });
                count++;
            }

        }

        public List<dtinfo> SelectKeyRelData(DateTime startDate, DateTime endDate, string dvName, string keyStat) {

            List<dtinfo> processedDtInfoAll = StaticAttribute.Function.processingDBData.ProcessingDtInfoAll(StaticAttribute.Function.selectDtInfoAllUseCase.Execute());
            List<dtinfo> processedDtInfoList = new List<dtinfo>();
            foreach(var dtInfoData in processedDtInfoAll) {
                if(dtInfoData.timestamp >= startDate && dtInfoData.timestamp <= endDate) {
                    if(dvName.Length ==0) {
                        if (keyStat.ToString().Length == 0) {
                            processedDtInfoList.Add(dtInfoData);
                        } else if (keyStat.ToString().Equals("키 배포 대기") && dtInfoData.stat.Equals("0")) {
                            processedDtInfoList.Add(dtInfoData);
                        } else if (keyStat.ToString().Equals("키 배포 완료") && dtInfoData.stat.Equals("1")) {
                            processedDtInfoList.Add(dtInfoData);
                        } else if (keyStat.ToString().Equals("키 재배포 완료") && dtInfoData.stat.Equals("2")) {
                            processedDtInfoList.Add(dtInfoData);
                        } else if (keyStat.ToString().Equals("키 삭제") && dtInfoData.stat.Equals("3")) {
                            processedDtInfoList.Add(dtInfoData);
                        } else if(keyStat.ToString().Equals("갱신") && dtInfoData.stat.Equals("4")) {
                            processedDtInfoList.Add(dtInfoData);
                        }
                    } else {
                        if(dtInfoData.mdid.Equals(dvName)) {
                            if (keyStat.ToString().Length == 0) {
                                processedDtInfoList.Add(dtInfoData);
                            } else if (keyStat.ToString().Equals("키 배포 대기") && dtInfoData.stat.Equals("0")) {
                                processedDtInfoList.Add(dtInfoData);
                            } else if (keyStat.ToString().Equals("키 배포 완료") && dtInfoData.stat.Equals("1")) {
                                processedDtInfoList.Add(dtInfoData);
                            } else if (keyStat.ToString().Equals("키 재배포 완료") && dtInfoData.stat.Equals("2")) {
                                processedDtInfoList.Add(dtInfoData);
                            } else if (keyStat.ToString().Equals("키 삭제") && dtInfoData.stat.Equals("3")) {
                                processedDtInfoList.Add(dtInfoData);
                            } else if (keyStat.ToString().Equals("갱신") && dtInfoData.stat.Equals("4")) {
                                processedDtInfoList.Add(dtInfoData);
                            }
                        }
                    }
                }
            }
            return processedDtInfoList;
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
