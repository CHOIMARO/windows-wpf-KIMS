using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.JSON.KIS100;
using KISM.DAO.JSON.KISM3;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.RequestFromKIS_100;
using KISM.ViewModel.SubPageVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace KISM.View.SubPage {
    /// <summary>
    /// KeyDistributionStatusPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyDistributionStatusPage : Page, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        KeyDistributionStatusPageVM keyDistributionStatusPageVM;
        List<FromKIS100Log> fromKIS100LogList = new List<FromKIS100Log>();
        public KeyDistributionStatusPage() {
            InitializeComponent();
            keyDistributionStatusPageVM = new KeyDistributionStatusPageVM();
            DataContext = keyDistributionStatusPageVM;
            keyDistributionStatusPageVM.ShowRegisteredData();
            keyDistributionStatusPageVM.Init();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            keyDistributionStatusPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            if (injectorDataGrid.Items.Count > 0) {
                injectorDataGrid.ScrollIntoView(injectorDataGrid.Items[injectorDataGrid.Items.Count - 1]);
            }
            keyDistributionStatusPageVM.DistributionLogReq();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            keyDistributionStatusPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            if(StaticAttribute.Function.movePageHistorySave) {
                NavigationService.RemoveBackEntry();
                StaticAttribute.Function.movePageHistorySave = false;
            }
            NavigationService.GoBack();
            keyDistributionStatusPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 배포 관리 페이지에서 뒤로 가기 버튼 클릭");
        }
        private void RegistBtn_Click(object sender, RoutedEventArgs e) {
            keyDistributionStatusPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 버튼 클릭");
            keyDistributionStatusPageVM.DistributionLogReg(fromKIS100LogList);
            if (IvUsers.Items.Count > 0) {
                if(injectorDataGrid.Items.Count>0) {
                    injectorDataGrid.ScrollIntoView(injectorDataGrid.Items[injectorDataGrid.Items.Count - 1]);
                }
            }
        }
        private void InitializeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyDistributionStatusPage.Initialize Button Click]");
            keyDistributionStatusPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "주입기 이력 초기화 버튼 클릭");
            if(StaticAttribute.Function.tcpConnect == 1) {
                keyDistributionStatusPageVM.ShowHistoryInitializePage();
            } else {
                InformationMessage.InformationShowDialog("주입기 상호 인증이 완료되지 않았습니다.");
            }
           
        }

        private void LoadHistoryBtn_Click(object sender, RoutedEventArgs e) {
            keyDistributionStatusPageVM.DistributionLogReq();
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {
            
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            if (registHistoryBtn.IsMouseOver) {
                registHistoryBtn.BorderBrush = new SolidColorBrush(Colors.White);
                registHistoryBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                registHistoryBtn_text.Foreground = new SolidColorBrush(Colors.White);
            } else if (initializeInjectorHistoryBtn.IsMouseOver) {
                initializeInjectorHistoryBtn.BorderBrush = new SolidColorBrush(Colors.White);
                initializeInjectorHistoryBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                initializeInjectorHistoryBtn_text.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            if (!registHistoryBtn.IsMouseOver) {
                registHistoryBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                registHistoryBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                registHistoryBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            }
            if (!initializeInjectorHistoryBtn.IsMouseOver) {
                initializeInjectorHistoryBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                initializeInjectorHistoryBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                initializeInjectorHistoryBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            }
        }

        public void OnNext(ReceivedFromKISDAO value) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                switch (value.type) {
                    case typeEnum.RES:
                        switch (value.cmd) {
                            case commandEnum.LOG:
                                if(fromKIS100LogList.Count > 0 && value.msg[0].idx ==0) {
                                    fromKIS100LogList.Clear();
                                }
                                foreach(var fromKIS100Log in value.msg) {
                                    fromKIS100LogList.Add(new FromKIS100Log {
                                        name =fromKIS100Log.name,
                                        timestamp = fromKIS100Log.timestamp,
                                        ip = fromKIS100Log.ip == null ? "-": fromKIS100Log.ip,
                                        stat = fromKIS100Log.stat,
                                        grp = fromKIS100Log.grp,
                                        hw = fromKIS100Log.hw,
                                        ppo = fromKIS100Log.ppo,
                                        sn = fromKIS100Log.sn
                                    });
                                }
                                StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.LOG, new FromKISM3Log {
                                    idx = fromKIS100LogList.Count
                                });
                                
                                break;
                            case commandEnum.DLOG:
                                if(value.msg.stat.Equals("Y")) {
                                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                                    fromKIS100LogList.Clear();
                                    keyDistributionStatusPageVM.ShowModuleData(fromKIS100LogList);
                                    InformationMessage.InformationShowDialog("주입기의 이력을 초기화했습니다.");
                                }
                                break;
                        }
                        break;
                    case typeEnum.REQ:
                        switch (value.cmd) {
                            case commandEnum.GEN:
                                RequestKeyGenerationPage requestKeyGenerationPage = new RequestKeyGenerationPage();
                                requestKeyGenerationPage.ShowDialog();

                                if (StaticAttribute.Function.movePageKeyGen) { //키 생성 페이지 이동
                                    KeyRegistrationManagementPage keyRegistrationManagementPage = new KeyRegistrationManagementPage();
                                    NavigationService.Navigate(keyRegistrationManagementPage);

                                    StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyRegistrationManagementPage]");
                                    keyDistributionStatusPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
                                }
                                break;
                            case commandEnum.RLOG:
                                StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.LOG, new FromKISM3Log {
                                    idx = 0
                                });
                                break;
                        }
                        break;
                    case typeEnum.END:
                        switch (value.cmd) {
                            case commandEnum.LOG:
                                foreach (var fromKIS100Log in value.msg) {
                                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                                    fromKIS100LogList.Add(new FromKIS100Log {
                                        name = fromKIS100Log.name,
                                        timestamp = fromKIS100Log.timestamp,
                                        ip = fromKIS100Log.ip == null ? "-" : fromKIS100Log.ip,
                                        stat = fromKIS100Log.stat,
                                        grp = fromKIS100Log.grp,
                                        hw = fromKIS100Log.hw,
                                        ppo = fromKIS100Log.ppo,
                                        sn = fromKIS100Log.sn
                                    });
                                }
                                keyDistributionStatusPageVM.ShowModuleData(fromKIS100LogList);
                                InformationMessage.InformationShowDialog("암호키 배포 이력을 가져왔습니다.");
                                break;
                        }
                        break;
                }
            }));
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }
        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {

        }
    }
}
