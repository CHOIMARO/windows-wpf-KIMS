using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.Ppose;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
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
    /// PurposeSettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PurposeSettingPage : Page, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        PurposeSettingPageVM purposeSettingPageVM;
        public PurposeSettingPage() {
            InitializeComponent();
            purposeSettingPageVM = new PurposeSettingPageVM();
            DataContext = purposeSettingPageVM;
            purposeSettingPageVM.ShowRegisteredData();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            purposeSettingPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            FocusingLastLine();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            purposeSettingPageVM.UnsetObserver(this);
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.GoBackPage]");
            purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 설정 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Add Button Click]");
            purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 추가 버튼 클릭");
            if (registPurposeTxt.Text.Trim().Length > 0) {
                bool state = purposeSettingPageVM.AddPposeInfoItem(registPurposeTxt.Text.Trim());

                if (state) {
                    InformationMessage.InformationShowDialog("용도를 등록했습니다.");
                    registPurposeTxt.Text = "";
                    purposeSettingPageVM.ShowRegisteredData();
                    FocusingLastLine();
                } else {
                    InformationMessage.InformationShowDialog("중복된 용도가 존재합니다.");
                }
            } else {
                InformationMessage.InformationShowDialog("등록할 용도를 입력하세요.");
            }

        }

        private void DelBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Delete Button Click]");
            purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 삭제 버튼 클릭");
            if (PposeDataGrid.SelectedIndex != -1) {
                PposeInfoDAO selectedRow = (PposeInfoDAO)PposeDataGrid.SelectedItem;
                bool state = purposeSettingPageVM.DeletePposeInfoItem(selectedRow);

                if (state) {
                    InformationMessage.InformationShowDialog("선택된 용도가 삭제되었습니다.");
                    purposeSettingPageVM.ShowRegisteredData();
                    FocusingLastLine();
                }
            } else {
                InformationMessage.InformationShowDialog("삭제할 용도를 선택해주세요");
                StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.You Have Not Selected Any Purpose To Delete]");
                purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.WARN, "삭제할 용도를 선택하지 않았습니다.");
            }
        }
        private void UpdateBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Change Button Click]");
            purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 변경 버튼 클릭");
            if (PposeDataGrid.SelectedIndex != -1) {
                PposeInfoDAO selectedRow = (PposeInfoDAO)PposeDataGrid.SelectedItem;
                purposeSettingPageVM.UpdatePposeInfoItem(selectedRow);
                purposeSettingPageVM.ShowRegisteredData();
                FocusingLastLine();
            } else {
                InformationMessage.InformationShowDialog("변경할 용도를 선택해주세요");
                StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.You Have Not Selected Any Purpose To Change]");
                purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.WARN, "변경할 용도를 선택하지 않았습니다.");
            }
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Home Button Click]");
            purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "홈 버튼 클릭");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }
        public void OnNext(TcpIsConnectDAO value) {
            
        }
        public void OnNext(ReceivedFromKISDAO value) {
            ClassifyMessage(value);
        }

        private void ClassifyMessage(ReceivedFromKISDAO value) {
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
                                    purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
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
                                    purposeSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 페이지로 이동");
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
        public void FocusingLastLine() {
            if (PposeDataGrid.Items.Count > 0) {
                PposeDataGrid.ScrollIntoView(PposeDataGrid.Items[PposeDataGrid.Items.Count - 1]);
            }
        }
    }
}
