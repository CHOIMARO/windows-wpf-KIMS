using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.RequestFromKIS_100;
using KISM.View.Information;
using KISM.ViewModel.SubPageVM;
using System;
using System.Collections.Generic;
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
    /// ManagerRegistrationManagementPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManagerRegistrationManagementPage : Page,
        IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        ManagerRegistrationManagementPageVM managerRegistrationManagementPageVM;
        public ManagerRegistrationManagementPage() {
            InitializeComponent();
            managerRegistrationManagementPageVM = new ManagerRegistrationManagementPageVM();
            DataContext = managerRegistrationManagementPageVM;

            managerRegistrationManagementPageVM.Init();

            managerRegistrationManagementPageVM.ShowRegisteredData();
            if (StaticAttribute.Function.connectedInjectorDAO.name != null && StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-100")) {
                managerRegistrationManagementPageVM.ShowConnectKIS100Window();
            } else if (StaticAttribute.Function.connectedInjectorDAO.name != null && StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200")) {
                managerRegistrationManagementPageVM.ShowConnectKIS200Window();
            } else if (StaticAttribute.Function.connectedInjectorDAO.name == null) {
                managerRegistrationManagementPageVM.ShowDefaultWindow();
            }
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            managerRegistrationManagementPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            if (injectorDataGrid.Items.Count > 0) {
                injectorDataGrid.ScrollIntoView(injectorDataGrid.Items[injectorDataGrid.Items.Count - 1]);
            }
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            managerRegistrationManagementPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            managerRegistrationManagementPageVM.InsertLog(LogEnum.INFO, "관리자 등록 관리 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnNext(TcpIsConnectDAO value) {
            if(value.stat == 2 || value.stat == 0) {
                managerRegistrationManagementPageVM.ShowDefaultWindow();
                managerRegistrationManagementPageVM.ShowRegisteredData();
            } else if (value.stat == 1) {
              
            }
            
        }

        public void OnNext(ReceivedFromKISDAO value) {
            ClassifyMessage(value);
        }

        private void ClassifyMessage(ReceivedFromKISDAO value) {
            switch (value.type) {
                case typeEnum.RES:
                    switch (value.cmd) {
                        case commandEnum.HELLO:
                            if (StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-100")) {
                                managerRegistrationManagementPageVM.ShowConnectKIS100Window();
                            } else if (StaticAttribute.Function.connectedInjectorDAO.name.Equals("KIS-200")) {
                                managerRegistrationManagementPageVM.ShowConnectKIS200Window();
                            }
                            break;
                        case commandEnum.UREG:
                            if(value.msg.stat.Equals("Y")) {
                                StaticAttribute.Function.loadingMessage.loadingViewClose();
                                InformationMessage.InformationShowDialog("주입기 계정 설정을 완료했습니다.");
                                StaticAttribute.Function.accountState = true;
                                managerRegistrationManagementPageVM.InsertInjectorMgrItem(StaticAttribute.Function.injectorID, StaticAttribute.Function.injectorPW);
                                managerRegistrationManagementPageVM.ShowRegisteredData();
                            }
                            break;
                        case commandEnum.UDEL:
                            if(value.msg.stat.Equals("Y")) {
                                StaticAttribute.Function.loadingMessage.loadingViewClose();
                                InformationMessage.InformationShowDialog("주입기 계정 삭제를 완료했습니다.");
                                StaticAttribute.Function.accountState = false;
                                managerRegistrationManagementPageVM.ShowRegisteredData();
                            }
                            break;
                        case commandEnum.UCK:
                            if(value.msg.stat.Equals("Y")) {
                                managerRegistrationManagementPageVM.ShowRegisteredData();
                            }
                            break;
                    }
                    break;
                case typeEnum.REQ:
                    switch (value.cmd) {
                        case commandEnum.GEN:
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                                RequestKeyGenerationPage requestKeyGenerationPage = new RequestKeyGenerationPage();
                                requestKeyGenerationPage.ShowDialog();

                                if (StaticAttribute.Function.movePageKeyGen) { //키 생성 페이지 이동
                                    KeyRegistrationManagementPage keyRegistrationManagementPage = new KeyRegistrationManagementPage();
                                    NavigationService.Navigate(keyRegistrationManagementPage);
                                    
                                    StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyRegistrationManagementPage]");
                                    managerRegistrationManagementPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
                                }
                            }));
                            break;
                        case commandEnum.RLOG:
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                                RequestHistorySavePage requestHistorySavePage = new RequestHistorySavePage();
                                requestHistorySavePage.ShowDialog();

                                if (StaticAttribute.Function.movePageHistorySave) { //이력 등록 페이지 이동
                                    KeyDistributionStatusPage keyDistributionStatusPage = new KeyDistributionStatusPage();
                                    NavigationService.Navigate(keyDistributionStatusPage);

                                    StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyDistributionStatusPage]");
                                    managerRegistrationManagementPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 페이지로 이동");
                                }
                            }));
                            break;

                    }
                    break;
                case typeEnum.END:
                    break;
            }
        }
    }
}
