using KISM.DAO;
using KISM.DAO.MilUnit;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.Setting {
    class MilitaryUnitSettingPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        int addCount = 0;
        List<MilUnitInfoDAO> milUnitInfoDAOList = new List<MilUnitInfoDAO>();
        List<milunitinfo> infoList;
        private ObservableCollection<MilUnitInfoDAO> milUnitDataRow = new ObservableCollection<MilUnitInfoDAO>();
        public ObservableCollection<MilUnitInfoDAO> MilUnitDataRow {
            get {
                return milUnitDataRow;
            }
            set {
                milUnitDataRow = value;
                onPropertyChanged("MilUnitDataRow");
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
        public void setObserver(dynamic info) {
            StaticAttribute.Function.loginExtensionTracker.Subscribe(info);
            StaticAttribute.Function.loginTimerTracker.Subscribe(this);
        }
        internal void setRow() {
            var info = StaticAttribute.ConstAttribute.userInfo;
            MilUnitDataRow.Add(new MilUnitInfoDAO {
                Num = "자동",
                Grp = "",
                Rank = info.rank,
                UserName = info.uname,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd tt h:mm:ss"),
                UniNum = info.uninum,
                Stat = "생성 예정",
                
            });
            addCount++;
        }

        internal void deleteRow(MilUnitInfoDAO selectedRow) {
            var info = infoList.Where(w => w.unit.Equals(selectedRow.Grp) && w.uninum.Equals(selectedRow.UniNum));
            try {
                bool state = StaticAttribute.Function.deleteMilitaryUnitUsecase.excute(info.First().idx);
                if (state) {
                    loadingList();
                    showRegisteredData();
                    InformationMessage.InformationShowDialog("부대 삭제가 완료되었습니다.");
                    StaticAttribute.Function.logCommand.infoLog("[VM.MilitaryUnitSettingPage.Delete MilitaryUnit Success]");
                    insertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 삭제 완료");
                }
            } catch (InvalidOperationException) {
                loadingList();
                showRegisteredData();
            }
        }

        internal void militaryUnitSaveEvent() {
            List<MilUnitInfoDAO> rows = new List<MilUnitInfoDAO>();
            var milUnitList = MilUnitDataRow;
            if (checkRows(milUnitList)) {
                if (checkDuplicateData(milUnitList)) {
                    foreach (var unit in milUnitList) {
                        if (!unit.Grp.Equals("")) {
                            rows.Add(unit);
                        }
                    }
                    //await Task.Run(() => StaticAttribute.Function.insertMilitaryUnitUsecase.excute(rows));
                    StaticAttribute.Function.insertMilitaryUnitUsecase.excute(rows);
                    loadingList();
                    showRegisteredData();
                } else {
                    InformationMessage.InformationShowDialog("중복된 부대가 존재합니다.");
                    StaticAttribute.Function.logCommand.infoLog("[VM.MilitaryUnitSetting.A Duplicate MilitaryUnit Exists]");
                    insertLog(StaticAttribute.Enum.LogEnum.WARN, "중복된 부대가 존재합니다.");
                }
            } else {
                    InformationMessage.InformationShowDialog("입력되지 않은 정보가 존재합니다.");
                StaticAttribute.Function.logCommand.infoLog("[VM.MilitaryUnitSetting.Incomplete Information Exists]");
                insertLog(StaticAttribute.Enum.LogEnum.WARN, "입력되지 않은 정보가 존재합니다.");
            }
        }

        public bool checkRows(ObservableCollection<MilUnitInfoDAO> observableCollection) {
            bool state = true;
            foreach(var unit in observableCollection) {
                if(unit.Grp.Equals("")) {
                    state = false;
                }
            }
            return state;
        }
        public bool checkDuplicateData (ObservableCollection<MilUnitInfoDAO> observerCollection) {
            return StaticAttribute.Function.saveCheckMilUnitInfoUsecase.excute(observerCollection);
        }
        public void loadingList() {
            int count = 0;
            milUnitInfoDAOList.Clear();
            infoList = StaticAttribute.Function.selectMilitaryUnitUsecase.excute();
            foreach (var info in infoList) {
                milUnitInfoDAOList.Add(new MilUnitInfoDAO {
                    Idx = info.idx,
                    Grp = info.unit,
                    Rank = info.rank,
                    Timestamp = info.timestamp.ToString(),
                    UserName = info.uname,
                    UniNum = info.uninum,
                    Stat = info.stat,
                });
            }
        }
        public void showRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                MilUnitDataRow.Clear();
                addColumnData();
            }));
        }
        private void addColumnData() {

            foreach (var milUnitInfo in milUnitInfoDAOList) {
                string state = milUnitInfo.Stat.Equals("A") ? "활성화" :
                    milUnitInfo.Stat.Equals("D") ? "삭제" : "알 수 없음";
                MilUnitDataRow.Add(new MilUnitInfoDAO {
                    Num = (MilUnitDataRow.Count+1).ToString(),
                    Idx = milUnitInfo.Idx,
                    Grp = milUnitInfo.Grp,
                    Rank = milUnitInfo.Rank,
                    Timestamp = milUnitInfo.Timestamp,
                    UserName = milUnitInfo.UserName,
                    UniNum = milUnitInfo.UniNum,
                    Stat = state,
                });
            }
        }
        public bool insertLog(LogEnum logEnum, string message) {
            return StaticAttribute.Function.insertLogInfoUsecase.excute(logEnum, message);
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