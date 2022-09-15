using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.View.Function.Logout;
using KISM.View.Function.RequestFromKIS_100;
using KISM.View.Setting;
using KISM.View.SubPage;
using KISM.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
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

namespace KISM.View {
    /// <summary>
    /// MainPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MainPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        MainPageVM mainPageVM;

        public MainPage() {
            InitializeComponent();

            mainPageVM = new MainPageVM();
            DataContext = mainPageVM;
            mainPageVM.CheckExpirationKey();
            mainPageVM.CheckingExpirationKeyTimer();
            mainPageVM.InitOverExpTooltip();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            mainPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            mainPageVM.Init();

        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            mainPageVM.UnsetObserver(this);
        }
        private void KeyRegistrationManagementBtn_Click(object sender, RoutedEventArgs e) {
            KeyRegistrationManagementPage keyRegistrationManagementPage = new KeyRegistrationManagementPage();
            NavigationService.Navigate(keyRegistrationManagementPage);
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyRegistrationManagementPage]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
        }

        private void KeyDistributionStatusBtn_Click(object sender, RoutedEventArgs e) {
            KeyDistributionStatusPage keyDistributionStatusPage = new KeyDistributionStatusPage();
            NavigationService.Navigate(keyDistributionStatusPage);
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyDistributionStatusPage]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 배포 관리 페이지로 이동");
        }

        private void ManagerRegistrationManagementBtn_Click(object sender, RoutedEventArgs e) {
            ManagerRegistrationManagementPage managerRegistrationManagementPage = new ManagerRegistrationManagementPage();
            NavigationService.Navigate(managerRegistrationManagementPage);
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To ManagerRegistrationManagementPage]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "관리자 등록 관리 페이지로 이동");
        }
        private void RecordStatusViewBtn_Click(object sender, RoutedEventArgs e) {
            RecordStatusViewPage recordStatusViewPage = new RecordStatusViewPage();
            NavigationService.Navigate(recordStatusViewPage);
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To RecordStatusViewPage]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 관리 페이지로 이동");
        }
        private void MilitaryUnitSetBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.SettingPage.MilitaryUnitSet Button Click]");
            mainPageVM.InsertLog(LogEnum.INFO, "부대 설정 페이지로 이동");
            MilitaryUnitSettingPage militaryUnitSettingPage = new MilitaryUnitSettingPage();
            NavigationService.Navigate(militaryUnitSettingPage);
        }

        private void PurposeSetBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.SettingPage.PurposeSet Button Click]");
            mainPageVM.InsertLog(LogEnum.INFO, "용도 설정 페이지로 이동");
            PurposeSettingPage purposeSettingPage = new PurposeSettingPage();
            NavigationService.Navigate(purposeSettingPage);
        }

        private void LogListBtn_Click(object sender, RoutedEventArgs e) {
            LogListPage logListPage = new LogListPage();
            NavigationService.Navigate(logListPage);
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "로그 이력 관리 페이지로 이동");
        }

        private void LogoutBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Logout Button Click]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "사용자 로그아웃");
            LogoutPage logoutPage = new LogoutPage();
            logoutPage.ShowDialog();

            if (StaticAttribute.Function.logoutPageState) {
                StaticAttribute.Function.logoutPageState = false;
                StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Log Out]");
                mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "메인 페이지 로그아웃");
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }

        private void SettingBtn_Click(object sender, RoutedEventArgs e) {
            SettingPage settingPage = new SettingPage();
            NavigationService.Navigate(settingPage);
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To SettingPage]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "세팅 페이지로 이동");
        }

        private void ExpirationBtn_Click(object sender, RoutedEventArgs e) {
            KeyRegistrationManagementPage keyRegistrationManagementPage = new KeyRegistrationManagementPage();
            NavigationService.Navigate(keyRegistrationManagementPage);
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Click Expiration Button]");
            StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyRegistrationManagementPage]");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "만료일 표시 버튼 클릭");
            mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
        }

        private void ConnectBtn_Click(object sender, RoutedEventArgs e) {
            mainPageVM.ConnectBtnClick();
        }
        public void OnNext(TcpIsConnectDAO value) {
            if (value.stat == 1) {
                Console.WriteLine("연결됨[MainPageVM]");
                mainPageVM.CurrentConnectedStatus();
            } else if (value.stat == 0) {
                Console.WriteLine("주입기와 연결 끊김[MainPageVM]");
                mainPageVM.CurrentNotConnectedStatus();
            } else if (value.stat == 2) {
                mainPageVM.CurrentTryToConnectStatus();

            }
        }
        public void OnNext(ReceivedFromKISDAO value) {
            ClassifyMessage(value);
        }

        private void ClassifyMessage(ReceivedFromKISDAO value) {
            switch (value.type) {
                case typeEnum.RES:
                    break;
                case typeEnum.REQ:
                    switch (value.cmd) {
                        case commandEnum.GEN:
                            //CreateKeyRequestWindow = Visibility.Visible;

                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                                RequestKeyGenerationPage requestKeyGenerationPage = new RequestKeyGenerationPage();
                                requestKeyGenerationPage.ShowDialog();

                                if (StaticAttribute.Function.movePageKeyGen) { //페이지 이동
                                    StaticAttribute.Function.movePageKeyGen = false;
                                    KeyRegistrationManagementPage keyRegistrationManagementPage = new KeyRegistrationManagementPage();
                                    NavigationService.Navigate(keyRegistrationManagementPage);

                                    StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyRegistrationManagementPage]");
                                    mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");

                                }
                            }));
                            break;
                        case commandEnum.RLOG:
                            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                                RequestHistorySavePage requestHistorySavePage = new RequestHistorySavePage();
                                requestHistorySavePage.ShowDialog();

                                if (StaticAttribute.Function.movePageHistorySave) { //이력 등록 페이지 이동
                                    StaticAttribute.Function.movePageHistorySave = false;
                                    KeyDistributionStatusPage keyDistributionStatusPage = new KeyDistributionStatusPage();
                                    NavigationService.Navigate(keyDistributionStatusPage);
                                    StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyDistributionStatusPage]");
                                    mainPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 페이지로 이동");
                                }
                            }));
                            break;

                    }
                    break;
                case typeEnum.END:
                    break;
            }
        }
        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {

        }
    }
}
