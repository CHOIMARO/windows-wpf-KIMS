using KISM.DAO;
using KISM.DAO.Ppose;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.MilUnitInfo;
using KISM.View.Function.PposeInfo;
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

namespace KISM.ViewModel.SubPageVM {
    class PurposeSettingPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        List<PposeInfoDAO> pposeInfoDAOList = new List<PposeInfoDAO>();
        List<pposeinfo> infoList = new List<pposeinfo>();

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
        public void ShowRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                PposeDataRow.Clear();
                AddColumnData();
            }));
        }
        private void AddColumnData() {
            var pposeInfoAll = StaticAttribute.Function.selectPposeInfoAllUseCase.Execute();

            foreach (var pposeInfoItem in pposeInfoAll) {
                string state = pposeInfoItem.stat.Equals("A") ? "활성화" :
                    pposeInfoItem.stat.Equals("D") ? "삭제" : "알 수 없음";

                if(pposeInfoItem.stat.Equals("A")) {
                    PposeDataRow.Add(new PposeInfoDAO {
                        Num = (PposeDataRow.Count + 1).ToString(),
                        Idx = pposeInfoItem.idx,
                        Ppose = pposeInfoItem.ppose,
                        Rank = pposeInfoItem.rank,
                        Timestamp = pposeInfoItem.timestamp.ToString(),
                        UserName = pposeInfoItem.uname,
                        UniNum = pposeInfoItem.uninum,
                        Stat = state,
                    });
                }
            }
        }
        internal bool DeleteMilUnitInfoItem(PposeInfoDAO selectedRow) {
            DeletePposeInfoItemPage deletepposeInfoItemPage = new DeletePposeInfoItemPage(selectedRow.Ppose);
            deletepposeInfoItemPage.ShowDialog();

            if (StaticAttribute.Function.deletePposeInfoItemPageState) {
                StaticAttribute.Function.deletePposeInfoItemPageState = false;
                return StaticAttribute.Function.deletePposeInfoItemUseCase.Execute(selectedRow.Idx);
            } else {
                return false;
            }
        }
        internal void UpdateMilUnitInfoItem(PposeInfoDAO selectedRow) {
            UpdatePposeInfoItemPage updatePposeInfoItemPage = new UpdatePposeInfoItemPage(selectedRow);
            updatePposeInfoItemPage.ShowDialog();
        }

        internal bool AddPposeInfoItem(string registPposeTxt) {
            var pposeInfoAll = StaticAttribute.Function.selectPposeInfoAllUseCase.Execute();

            foreach (var pposeInfoItem in pposeInfoAll) {
                if (pposeInfoItem.ppose.Equals(registPposeTxt) && pposeInfoItem.stat.Equals("A")) {
                    return false;
                }
            }
            return StaticAttribute.Function.insertPposeInfoItemUseCase.Execute(registPposeTxt);
        }
        internal bool DeletePposeInfoItem(PposeInfoDAO selectedRow) {
            DeletePposeInfoItemPage deletePposeInfoItemPage = new DeletePposeInfoItemPage(selectedRow.Ppose);
            deletePposeInfoItemPage.ShowDialog();

            if (StaticAttribute.Function.deletePposeInfoItemPageState) {
                StaticAttribute.Function.deletePposeInfoItemPageState = false;
                return StaticAttribute.Function.deletePposeInfoItemUseCase.Execute(selectedRow.Idx);
            } else {
                return false;
            }
        }

        internal void UpdatePposeInfoItem(PposeInfoDAO selectedRow) {
            UpdatePposeInfoItemPage updatePposeInfoItemPage = new UpdatePposeInfoItemPage(selectedRow);
            updatePposeInfoItemPage.ShowDialog();
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
