using KISM.DAO;
using KISM.DAO.Ppose;
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

namespace KISM.ViewModel.Setting {
    class PurposeSettingPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        List<PposeInfoDAO> pposeInfoDAOList = new List<PposeInfoDAO>();
        List<pposeinfo> infoList;
        int addCount = 0;

        private ObservableCollection<PposeInfoDAO> pposeDataRow = new ObservableCollection<PposeInfoDAO>();
        public ObservableCollection<PposeInfoDAO> PposeDataRow {
            get {
                return pposeDataRow;
            }
            set {
                pposeDataRow = value;
                onPropertyChanged("PposeDataRow");
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
        public void loadingList() {
            int count = 0;
            pposeInfoDAOList.Clear();
            infoList = StaticAttribute.Function.selectPurposeInfoUsecase.excute();
            foreach (var info in infoList) {
                pposeInfoDAOList.Add(new PposeInfoDAO {
                    Num = count++.ToString(),
                    Idx = info.idx,
                    Ppose = info.ppose,
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
                PposeDataRow.Clear();
                addColumnData();
            }));
        }
        private void addColumnData() {

            foreach (var pposeInfo in pposeInfoDAOList) {
                string state = pposeInfo.Stat.Equals("A") ? "활성화" :
                    pposeInfo.Stat.Equals("D") ? "삭제" : "알 수 없음";
                PposeDataRow.Add(new PposeInfoDAO {
                    Num = (PposeDataRow.Count+1).ToString(),
                    Idx = pposeInfo.Idx,
                    Ppose = pposeInfo.Ppose,
                    Rank = pposeInfo.Rank,
                    Timestamp = pposeInfo.Timestamp,
                    UserName = pposeInfo.UserName,
                    UniNum = pposeInfo.UniNum,
                    Stat = state,
                });
            }
        }
        internal void setRow() {
            var info = StaticAttribute.ConstAttribute.userInfo;
            PposeDataRow.Add(new PposeInfoDAO {
                Num = "자동",
                Ppose = "",
                Rank = info.rank,
                UserName = info.uname,
                Timestamp = DateTime.Now.ToString("yyyy-MM-dd tt h:mm:ss"),
                UniNum = info.uninum,
                Stat = "생성 예정",

            });
            addCount++;
        }

        internal void deleteRow(PposeInfoDAO selectedRow) {
            var info = infoList.Where(w => w.ppose.Equals(selectedRow.Ppose) && w.uninum.Equals(selectedRow.UniNum));
            try {
                bool state = StaticAttribute.Function.deletePurposeInfoUsecase.excute(info.First().idx);
                if (state) {
                    loadingList();
                    showRegisteredData();
                    StaticAttribute.Function.logCommand.infoLog("[VM.PurposeSettingPage.Delete Purpose Success]");
                    InformationMessage.InformationShowDialog("용도 삭제가 완료되었습니다.");
                    insertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 삭제 완료");
                }
            } catch (InvalidOperationException) {
                loadingList();
                showRegisteredData();
            }
        }

        internal void purposeInfoSaveEvent() {
            List<PposeInfoDAO> rows = new List<PposeInfoDAO>();
            var pposeList = PposeDataRow;
            if (checkRows(pposeList)) {
                if (checkDuplicateData(pposeList)) {
                    foreach (var unit in pposeList) {
                        if (!unit.Ppose.Equals("")) {
                            rows.Add(unit);
                        }
                    }
                    StaticAttribute.Function.insertPurposeInfoUsecase.excute(rows);
                    loadingList();
                    showRegisteredData();
                } else {
                    StaticAttribute.Function.logCommand.infoLog("[VM.PurposeSettingPage.A Duplicate Purpose Exists]");
                    InformationMessage.InformationShowDialog("중복된 부대가 존재합니다.");
                    insertLog(StaticAttribute.Enum.LogEnum.WARN, "중복된 부대가 존재합니다.");
                }
            } else {
                StaticAttribute.Function.logCommand.infoLog("[VM.PurposeSettingPage.Incomplete Information Exists]");
                InformationMessage.InformationShowDialog("입력되지 않은 정보가 존재합니다.");
                insertLog(StaticAttribute.Enum.LogEnum.WARN, "입력되지 않은 정보가 존재합니다.");
            }
        }

        public bool checkRows(ObservableCollection<PposeInfoDAO> observableCollection) {
            bool state = true;
            foreach (var unit in observableCollection) {
                if (unit.Ppose.Equals("")) {
                    state = false;
                }
            }
            return state;
        }
        public bool checkDuplicateData(ObservableCollection<PposeInfoDAO> observerCollection) {
            return StaticAttribute.Function.saveCheckPurposeInfoUsecase.excute(observerCollection);
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
