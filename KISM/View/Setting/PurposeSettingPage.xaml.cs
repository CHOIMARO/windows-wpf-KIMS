using KISM.DAO;
using KISM.DAO.Ppose;
using KISM.Util;
using KISM.ViewModel.Setting;
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

namespace KISM.View.Setting {
    /// <summary>
    /// PurposeSettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class PurposeSettingPage : Page,IObserver<LoginExtensionDAO> {
        PurposeSettingPageVM purposeSettingPageVM;
        public PurposeSettingPage() {
            InitializeComponent();
            purposeSettingPageVM = new PurposeSettingPageVM();
            DataContext = purposeSettingPageVM;
            purposeSettingPageVM.loadingList();
            purposeSettingPageVM.showRegisteredData();
            purposeSettingPageVM.setObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.GoBackPage]");
            purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 설정 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Add Button Click");
            purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 추가 버튼 클릭");
            purposeSettingPageVM.setRow();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Delete Button Click");
            purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 삭제 버튼 클릭");
            if (PposeDataGrid.SelectedIndex != -1) {
                PposeInfoDAO selectedRow = (PposeInfoDAO)PposeDataGrid.SelectedItem;
                purposeSettingPageVM.deleteRow(selectedRow);
            } else {
                StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.You Have Not Selected Any Purposes To Delete");
                purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.WARN, "삭제할 용도를 선택하지 않았습니다.");
                InformationMessage.InformationShowDialog("삭제할 용도를 선택해주세요");
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Save Button Click");
            purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "용도 저장 버튼 클릭");
            purposeSettingPageVM.purposeInfoSaveEvent();
        }

        private void homeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.PurposeSettingPage.Home Button Click]");
            purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "홈 버튼 클릭");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
        }

        public void OnNext(LoginExtensionDAO value) {
            if (!value.state) {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                    if (NavigationService != null) {
                        NavigationService.RemoveBackEntry();
                        NavigationService.RemoveBackEntry();
                        NavigationService.GoBack();
                    }
                }));
                StaticAttribute.Function.logCommand.infoLog("[VI.purposeSettingPage.Logout]");
                purposeSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "로그아웃");
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }
    }
}
