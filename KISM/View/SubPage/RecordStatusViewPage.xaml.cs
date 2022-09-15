using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.View.Function.RequestFromKIS_100;
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
    /// RecordStatusViewPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RecordStatusViewPage : Page, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        RecordStatusViewPageVM recordStatusViewPageVM;
        public RecordStatusViewPage() {
            InitializeComponent();
            recordStatusViewPageVM = new RecordStatusViewPageVM();
            DataContext = recordStatusViewPageVM;
            pageGroup.SelectedIndex = 0;
            recordStatusViewPageVM.Init();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            recordStatusViewPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            recordStatusViewPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            recordStatusViewPageVM.InsertLog(LogEnum.INFO, "암호키 이력 관리 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void PageGroup_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            if(pageGroup.SelectedItem != null) {
                Console.WriteLine(pageGroup.SelectedItem.ToString());
                if(pageGroup.SelectedItem.ToString().Equals("암호키 등록 현황")) {
                    StaticAttribute.Function.logCommand.infoLog("[VI.RecordStatusViewPage.Go To KeyManagementRecordPage]");
                    recordStatusViewPageVM.InsertLog(LogEnum.INFO, "키 관리 이력 페이지로 이동");
                    recordStatusViewPageVM.ShowKeyManagementWindow();
                } else if(pageGroup.SelectedItem.ToString().Equals("암호키 배포 현황")) {
                    StaticAttribute.Function.logCommand.infoLog("[VI.RecordStatusViewPage.Go To KeyDistributionRecordPage]");
                    recordStatusViewPageVM.InsertLog(LogEnum.INFO, "키 분배 이력 페이지로 이동");
                    recordStatusViewPageVM.ShowKeyDistributionWindow();
                }
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }

        public void OnNext(ReceivedFromKISDAO value) {
            switch (value.type) {
                case typeEnum.RES:
                    switch (value.cmd) {

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
                                    recordStatusViewPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
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
                                    recordStatusViewPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 페이지로 이동");
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
