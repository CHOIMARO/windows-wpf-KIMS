using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.MilUnit;
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
    /// MilitaryUnitSettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MilitaryUnitSettingPage : Page, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        MilitaryUnitSettingPageVM militaryUnitSettingPageVM;
        public MilitaryUnitSettingPage() {
            InitializeComponent();
            militaryUnitSettingPageVM = new MilitaryUnitSettingPageVM();
            DataContext = militaryUnitSettingPageVM;
            militaryUnitSettingPageVM.ShowRegisteredData();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            militaryUnitSettingPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            FocusingLastLine();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            militaryUnitSettingPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.GoBackPage]");
            militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 설정 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Add Button Click]");
            militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 추가 버튼 클릭");
            if(registGroupTxt.Text.Trim().Length >0) {
                bool state = militaryUnitSettingPageVM.AddMilUnitInfoItem(registGroupTxt.Text.Trim());

                if (state) {
                    InformationMessage.InformationShowDialog("부대를 등록했습니다.");
                    registGroupTxt.Text = "";
                    militaryUnitSettingPageVM.ShowRegisteredData();
                    FocusingLastLine();
                } else {
                    InformationMessage.InformationShowDialog("중복된 부대가 존재합니다.");
                }
            } else {
                InformationMessage.InformationShowDialog("등록할 부대를 입력하세요.");
            }
            
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Delete Button Click]");
            militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 삭제 버튼 클릭");
            if (MilUnitDataGrid.SelectedIndex != -1) {
                MilUnitInfoDAO selectedRow = (MilUnitInfoDAO)MilUnitDataGrid.SelectedItem;
                bool state = militaryUnitSettingPageVM.DeleteMilUnitInfoItem(selectedRow);

                if(state) {
                    InformationMessage.InformationShowDialog("선택된 부대가 삭제되었습니다.");
                    militaryUnitSettingPageVM.ShowRegisteredData();
                    FocusingLastLine();
                }
            } else {
                InformationMessage.InformationShowDialog("삭제할 부대를 선택해주세요");
                StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.You Have Not Selected Any Groups To Delete]");
                militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.WARN, "삭제할 부대를 선택하지 않았습니다.");
            }
        }
        private void UpdateBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Change Button Click]");
            militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 변경 버튼 클릭");
            if (MilUnitDataGrid.SelectedIndex != -1) {
                MilUnitInfoDAO selectedRow = (MilUnitInfoDAO)MilUnitDataGrid.SelectedItem;
                militaryUnitSettingPageVM.UpdateMilUnitInfoItem(selectedRow);
                militaryUnitSettingPageVM.ShowRegisteredData();
                FocusingLastLine();
            } else {
                InformationMessage.InformationShowDialog("변경할 부대를 선택해주세요");
                StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.You Have Not Selected Any Groups To Change]");
                militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.WARN, "변경할 부대를 선택하지 않았습니다.");
            }
        }
        private void homeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Home Button Click]");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        private void groupDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

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
                                    militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 등록 관리 페이지로 이동");
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
                                    militaryUnitSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 페이지로 이동");
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
            if (MilUnitDataGrid.Items.Count > 0) {
                MilUnitDataGrid.ScrollIntoView(MilUnitDataGrid.Items[MilUnitDataGrid.Items.Count - 1]);
            }
        }
    }
}
