using KISM.DAO;
using KISM.DAO.KeyRel;
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
    public class KeyManagementRecordPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
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
            "전체 선택", "키 등록 완료", "키 배포 완료", "키 삭제"
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
        private ObservableCollection<KeyRelDAO> registeredDataRow = new ObservableCollection<KeyRelDAO>();
        public ObservableCollection<KeyRelDAO> RegisteredDataRow {
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

        public async Task<bool> ShowRegisteredData() {
            RegisteredDataRow.Clear();
            await Task.Delay(100);
            AddColumnData();
            return true;
        }
        private void AddColumnData() {
            var keyRelAll = StaticAttribute.Function.selectKeyRelAllUseCase.Execute();
            foreach (var keyRel in keyRelAll) {
                string state = keyRel.stat.Equals("REG") ? "키 등록 완료" :
                               keyRel.stat.Equals("WAIT") ? "키 재배포 완료" :
                               keyRel.stat.Equals("DT") ? "키 배포 완료" : "키 삭제";
                RegisteredDataRow.Add(new KeyRelDAO {
                    Num = RegisteredDataRow.Count+1,
                    Timestamp = keyRel.timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    StDate = keyRel.stdate.ToString("yyyy-MM-dd"),
                    ExpDate = keyRel.expdate.ToString("yyyy-MM-dd"),
                    Dpt = keyRel.dpt,
                    Ppose = keyRel.ppose,
                    Stat = state,
                    Wkey = keyRel.wkey,
                    Idx = keyRel.idx,
                    DelDate = keyRel.deldate == null ? "" : keyRel.deldate.ToString(),
                    DtDate = keyRel.dtdate == null ? "" : keyRel.dtdate.ToString(),
                    Ijidx = keyRel.ijidx,
                });
            }
        }
        public void SetGrp() {
            var milUnitInfoAll = StaticAttribute.Function.selectMilUnitInfoAllUseCase.Execute();
            GrpItems.Add("전체 선택");
            foreach(var milUnitInfo in milUnitInfoAll) {
                if(milUnitInfo.stat.Equals("A")) {
                    GrpItems.Add(milUnitInfo.unit);
                }
            }
            
        }
        public void SetPpose() {
            PposeItems.Add("전체 선택");
            var pposeInfoAll = StaticAttribute.Function.selectPposeInfoAllUseCase.Execute();
            foreach(var pposeInfo in pposeInfoAll) {
                if (pposeInfo.stat.Equals("A")) {
                    PposeItems.Add(pposeInfo.ppose);
                }
            }
        }

        public void CheckUserSearch(string datePickerStart, string datePickerEnd, string grp, string ppose, string keyStat) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                RegisteredDataRow.Clear();
            }));
            DateTime datePickerSt = DateTime.Now.Date;
            DateTime datePickerE = DateTime.Now.Date;
            string group = string.Empty;
            string purpose = string.Empty;
            string keyStatus = string.Empty;
            if(grp.Equals("전체 선택")) {
                group= "";
            } else {
                group = grp;
            }
            if(ppose.Equals("전체 선택")) {
                purpose = "";
            } else {
                purpose = ppose;
            }
            if(keyStat.Equals("전체 선택")) {
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

            List<keyrel> selectKeyRelList = SelectKeyRelData(datePickerSt, datePickerE, group, purpose, keyStatus);
            int count = 0;
            foreach (var keyRel in selectKeyRelList) {
                string state = keyRel.stat.Equals("REG") ? "키 등록 완료" :
                               keyRel.stat.Equals("WAIT") ? "키 재배포 완료" :
                               keyRel.stat.Equals("DT") ? "키 배포 완료" : "키 삭제";
                RegisteredDataRow.Add(new KeyRelDAO {
                    Num = count,
                    Timestamp = keyRel.timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                    StDate = keyRel.stdate.ToString("yyyy-MM-dd"),
                    ExpDate = keyRel.expdate.ToString("yyyy-MM-dd"),
                    Dpt = keyRel.dpt,
                    Ppose = keyRel.ppose,
                    Stat = state,
                    Wkey = keyRel.wkey,
                    Idx = keyRel.idx,
                    DelDate = keyRel.deldate.ToString(),
                    DtDate = keyRel.dtdate.ToString(),
                    Ijidx = keyRel.ijidx,
                });
                count++;
            }

        }

        public List<keyrel> SelectKeyRelData(DateTime startDate, DateTime endDate, string grp, string ppose, string keyStat) {
            List<keyrel> processedkeyRelAll = StaticAttribute.Function.processingDBData.ProcessingKeyRelAll(StaticAttribute.Function.selectKeyRelAllUseCase.Execute());
            List<keyrel> processedKeyRelList = new List<keyrel>();
            foreach (var keyRelData in processedkeyRelAll) {
                if (keyRelData.timestamp >= startDate && keyRelData.timestamp <= endDate) {
                    if (grp.Length == 0 && ppose.Length == 0) {
                        if (keyStat.ToString().Length == 0) {
                            processedKeyRelList.Add(keyRelData);
                        } else if (keyStat.ToString().Equals("키 등록 완료") && keyRelData.stat.Equals("REG")) {
                            processedKeyRelList.Add(keyRelData);
                        } else if (keyStat.ToString().Equals("키 삭제") && keyRelData.stat.Equals("DEL")) {
                            processedKeyRelList.Add(keyRelData);
                        } else if (keyStat.ToString().Equals("키 배포 완료") && keyRelData.stat.Equals("DT")) {
                            processedKeyRelList.Add(keyRelData);
                        }
                    } else if (grp.Length > 0 && ppose.Length == 0) {
                        if (keyRelData.dpt.Equals(grp)) {
                            if (keyStat.ToString().Length == 0) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 등록 완료") && keyRelData.stat.Equals("REG")) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 삭제") && keyRelData.stat.Equals("DEL")) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 배포 완료") && keyRelData.stat.Equals("DT")) {
                                processedKeyRelList.Add(keyRelData);
                            }
                        }
                    } else if (grp.Length == 0 && ppose.Length > 0) {
                        if (keyRelData.ppose.Equals(ppose)) {
                            if (keyStat.ToString().Length == 0) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 등록 완료") && keyRelData.stat.Equals("REG")) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 삭제") && keyRelData.stat.Equals("DEL")) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 배포 완료") && keyRelData.stat.Equals("DT")) {
                                processedKeyRelList.Add(keyRelData);
                            }
                        }
                    } else if (grp.Length > 0 && ppose.Length > 0) {
                        if (keyRelData.dpt.Equals(grp) && keyRelData.ppose.Equals(ppose)) {
                            if (keyStat.ToString().Length == 0) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 등록 완료") && keyRelData.stat.Equals("REG")) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 삭제") && keyRelData.stat.Equals("DEL")) {
                                processedKeyRelList.Add(keyRelData);
                            } else if (keyStat.ToString().Equals("키 배포 완료") && keyRelData.stat.Equals("DT")) {
                                processedKeyRelList.Add(keyRelData);
                            }
                        }
                    }
                }
            }
            return processedKeyRelList;
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
