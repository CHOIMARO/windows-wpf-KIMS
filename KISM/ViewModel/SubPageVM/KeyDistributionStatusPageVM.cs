using KISM.DAO;
using KISM.DAO.Distribution;
using KISM.DAO.JSON.KIS100;
using KISM.DAO.JSON.KISM3;
using KISM.DAO.KeyRel;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.HistoryInitialize;
using KISM.View.Loading;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KISM.ViewModel.SubPageVM {
    public class KeyDistributionStatusPageVM : INotifyPropertyChanged, IObserver<LoginTimerDAO> {
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

        private ObservableCollection<KIS100LOGString> moduleList = new ObservableCollection<KIS100LOGString>();
        public ObservableCollection<KIS100LOGString> ModuleList {
            get {
                return moduleList;
            }
            set {
                moduleList = value;
                onPropertyChanged("ModuleList");
            }
        }
        #endregion

        internal void Init() {

        }

        public void DistributionLogReq() {
            if(StaticAttribute.Function.authState) {
                if(StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-100") && StaticAttribute.Function.tcpConnect == 1) {
                    
                } else if(StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200") && StaticAttribute.Function.tcpConnect == 1) {
                    StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.LOG, new FromKISM3Log {
                        idx = 0,
                    });
                }
                
                StaticAttribute.Function.logCommand.debugLog("[VM.KeyDistributionStatusPage.Request Log List]");
            } else {
                InformationMessage.InformationShowDialog("주입기 상호 인증이 완료되지 않았습니다.");
            }
        }
        public async void DistributionLogReg(List<FromKIS100Log> logList) {
            List<FromKIS100Log> fromKIS100LogList = new List<FromKIS100Log>();
            foreach(var log in logList) {
                fromKIS100LogList.Add(new FromKIS100Log {
                    grp = log.grp,
                    hw = log.hw,
                    ip = log.ip,
                    name = log.name,
                    ppo = log.ppo,
                    sn = log.sn,
                    stat = log.stat,
                    timestamp = log.timestamp
                });
            }
            int state = -1;
            if(StaticAttribute.Function.authState) {
                if (fromKIS100LogList.Count > 0) {
                    List<keyrel> processedKeyRelAll = StaticAttribute.Function.processingDBData.ProcessingKeyRelAll(StaticAttribute.Function.selectKeyRelAllUseCase.Execute());
                    foreach (var logData in fromKIS100LogList) {
                        foreach (var keyRelItem in processedKeyRelAll) {
                            if (Int32.Parse(logData.grp) == (keyRelItem.idx)) {
                                logData.grp = keyRelItem.dpt;
                                break;
                            }
                        }
                    }
                    StaticAttribute.Function.loadingMessage.setMessage("이력 등록 중..");
                    state = await Task.Run(() => StaticAttribute.Function.insertDtInfoListUseCase.Execute(fromKIS100LogList, StaticAttribute.Function.selectDtInfoAllUseCase.Execute()));
                    StaticAttribute.Function.logCommand.debugLog("[VM.KeyDistributionStatusPage.Regist Key Distribution Status]");
                    InsertLog(LogEnum.INFO, "암호키 이력을 등록합니다.");

                    if (state == 1) {
                        StaticAttribute.Function.loadingMessage.loadingViewClose();
                        ShowRegisteredData();
                        InformationMessage.InformationShowDialog("암호키 이력을 등록했습니다.");
                        StaticAttribute.Function.logCommand.debugLog("[VM.KeyDistributionStatusPage.Regist Key Distribution Status Success]");
                        InsertLog(LogEnum.INFO, "암호키 이력을 등록했습니다.");
                        ShowHistoryInitializePage();
                    } else if (state == 0) {
                        StaticAttribute.Function.loadingMessage.loadingViewClose();
                        InformationMessage.InformationShowDialog("모든 이력이 이미 저장되어있습니다.");
                        StaticAttribute.Function.logCommand.warnLog("[VM.KeyDistributionStatusPage.Exist Duplicate Distribution Status]");
                        InsertLog(LogEnum.WARN, "모든 이력이 이미 저장되어있습니다.");
                        ShowHistoryInitializePage();
                    } else if (state == 2) {
                        StaticAttribute.Function.loadingMessage.loadingViewClose();
                        ShowRegisteredData();
                        InformationMessage.InformationShowDialog("중복된 이력을 제외하고 등록되었습니다.");
                        StaticAttribute.Function.logCommand.warnLog("[VM.KeyDistributionStatusPage.Regist Key Distribution Status Except Duplicate Content]");
                        InsertLog(LogEnum.WARN, "중복된 이력을 제외하고 등록되었습니다.");
                        ShowHistoryInitializePage();

                    }
                } else {
                    InformationMessage.InformationShowDialog("등록할 이력이 없습니다.");
                    StaticAttribute.Function.logCommand.warnLog("[VM.KeyDistributionStatusPage.There is no Distribution Status to register]");
                    InsertLog(LogEnum.WARN, "등록할 이력이 존재하지 않습니다.");
                }
            } else {
                InformationMessage.InformationShowDialog("주입기 상호 인증이 완료되지 않았습니다.");
                StaticAttribute.Function.logCommand.warnLog("[VM.KeyDistributionStatusPage.Injector Mutual Authentication Is Not Complete]");
                InsertLog(LogEnum.WARN, "주입기 상호 인증이 완료되지 않았습니다.");
            }
        }

        public void ShowHistoryInitializePage() {
            HistoryInitializePage historyInitializePage = new HistoryInitializePage();
            historyInitializePage.ShowDialog();

            if (StaticAttribute.Function.historyInitializePageState) {
                StaticAttribute.Function.loadingMessage.setMessage("이력 초기화 중..");
                StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.DLOG, "");
            }
        }
        public void ShowModuleData(List<FromKIS100Log> fromKIS100LogList) {
            List<keyrel> processedKeyRelAll = StaticAttribute.Function.processingDBData.ProcessingKeyRelAll(StaticAttribute.Function.selectKeyRelAllUseCase.Execute());
            
            ModuleList.Clear();
            foreach (var logData in fromKIS100LogList) {
                string state = logData.stat.Equals(0) ? "키 배포 대기" :
                    logData.stat.Equals(1) ? "키 배포 완료" :
                    logData.stat.Equals(2) ? "키 재배포 완료" :
                    logData.stat.Equals(3) ? "키 삭제" :
                    logData.stat.Equals(4) ? "IP 갱신" : "알 수없음";
                string group = string.Empty;

                foreach(var keyRelItem in processedKeyRelAll) {
                    if(Int32.Parse(logData.grp) == (keyRelItem.idx)) {
                        group = keyRelItem.dpt;
                        break;
                    }
                }
                ModuleList.Add(new KIS100LOGString() {
                    name = logData.name,
                    timestamp = logData.timestamp,
                    ip = logData.ip,
                    stat = state,
                    hw = logData.hw,
                    sn = logData.sn,
                    grp = group,
                    ppo = logData.ppo
                });
            }
            
        }
        public void ShowRegisteredData() {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                registeredDataRow.Clear();
                AddColumnData();
            }));

        }
        private void AddColumnData() {
            var dtInfoAll = StaticAttribute.Function.selectDtInfoAllUseCase.Execute();
            foreach(var dtInfo in dtInfoAll) {
                string state = dtInfo.stat.Equals("0") ? "키 배포 대기" :
                              dtInfo.stat.Equals("1") ? "키 배포 완료" :
                              dtInfo.stat.Equals("2") ? "키 재배포 완료" :
                              dtInfo.stat.Equals("3") ? "키 삭제" :
                              dtInfo.stat.Equals("4") ? "IP 갱신" : "알 수 없음";

                RegisteredDataRow.Add(new DtInfoDAO {
                    Num = (RegisteredDataRow.Count+1).ToString(),
                    RegDate = dtInfo.timestamp.ToString(),
                    DvName = dtInfo.hw,
                    Stat = state,
                    //Hw = dtInfo.sn,
                    Sn = dtInfo.sn,
                    Dpt = dtInfo.dpt,
                    Ppose = dtInfo.ppose
                });
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
