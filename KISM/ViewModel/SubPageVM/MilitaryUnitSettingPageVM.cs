using KISM.DAO;
using KISM.DAO.MilUnit;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.MilUnitInfo;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.SubPageVM {
    class MilitaryUnitSettingPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        List<MilUnitInfoDAO> milUnitInfoDAOList = new List<MilUnitInfoDAO>();
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
        internal bool AddMilUnitInfoItem(string registGroupTxt) {
            var milUnitInfoAll = StaticAttribute.Function.selectMilUnitInfoAllUseCase.Execute();

            foreach (var milUnitItem in milUnitInfoAll) {
                if (milUnitItem.unit.Equals(registGroupTxt) && milUnitItem.stat.Equals("A")) {
                    return false;
                }
            }
            return StaticAttribute.Function.insertMilUnitInfoItemUseCase.Execute(registGroupTxt);
        }
        internal bool DeleteMilUnitInfoItem(MilUnitInfoDAO selectedRow) {
            DeleteMilUnitInfoItemPage deleteMilUnitInfoItemPage = new DeleteMilUnitInfoItemPage(selectedRow.Grp);
            deleteMilUnitInfoItemPage.ShowDialog();

            if(StaticAttribute.Function.deleteMilUnitInfoItemPageState) {
                StaticAttribute.Function.deleteMilUnitInfoItemPageState = false;
                return StaticAttribute.Function.deleteMilUnitInfoItemUseCase.Execute(selectedRow.Idx);
            } else {
                return false;
            }
        }
        internal void UpdateMilUnitInfoItem(MilUnitInfoDAO selectedRow) {
            UpdateMilUnitInfoItemPage updateMilUnitInfoItemPage = new UpdateMilUnitInfoItemPage(selectedRow);
            updateMilUnitInfoItemPage.ShowDialog();
        }
        public void ShowRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                MilUnitDataRow.Clear();
                AddColumnData();
            }));
        }
        private void AddColumnData() {
            var milUnitInfoAll = StaticAttribute.Function.selectMilUnitInfoAllUseCase.Execute();

            foreach (var milUnitInfoItem in milUnitInfoAll) {
                string state = milUnitInfoItem.stat.Equals("A") ? "활성화" :
                    milUnitInfoItem.stat.Equals("D") ? "삭제" : "알 수 없음";

                if(milUnitInfoItem.stat.Equals("A")) {
                    MilUnitDataRow.Add(new MilUnitInfoDAO {
                        Num = (MilUnitDataRow.Count + 1).ToString(),
                        Idx = milUnitInfoItem.idx,
                        Grp = milUnitInfoItem.unit,
                        Rank = milUnitInfoItem.rank,
                        Timestamp = milUnitInfoItem.timestamp.ToString(),
                        UserName = milUnitInfoItem.uname,
                        UniNum = milUnitInfoItem.uninum,
                        Stat = state,
                    });
                }
            }
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