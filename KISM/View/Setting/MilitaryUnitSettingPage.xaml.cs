using KISM.DAO;
using KISM.DAO.MilUnit;
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
    /// MilitaryUnitSettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class MilitaryUnitSettingPage : Page, IObserver<LoginExtensionDAO> {
        MilitaryUnitSettingPageVM militaryUnitSettingPageVM;
        public MilitaryUnitSettingPage() {
            InitializeComponent();
            militaryUnitSettingPageVM = new MilitaryUnitSettingPageVM();
            DataContext = militaryUnitSettingPageVM;
            militaryUnitSettingPageVM.loadingList();
            militaryUnitSettingPageVM.showRegisteredData();
            militaryUnitSettingPageVM.setObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.GoBackPage]");
            militaryUnitSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 설정 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {

        }

        private void AddBtn_Click(object sender, RoutedEventArgs e) {
            militaryUnitSettingPageVM.setRow();
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Add Button Click]");
            militaryUnitSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 추가 버튼 클릭");
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Delete Button Click]");
            militaryUnitSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "부대 삭제 버튼 클릭");
            if (MilUnitDataGrid.SelectedIndex != -1) {
                MilUnitInfoDAO selectedRow = (MilUnitInfoDAO)MilUnitDataGrid.SelectedItem;
                militaryUnitSettingPageVM.deleteRow(selectedRow);
            } else {
                InformationMessage.InformationShowDialog("삭제할 부대를 선택해주세요");
                StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.You Have Not Selected Any Groups To Delete]");
                militaryUnitSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.WARN, "삭제할 부대를 선택하지 않았습니다.");
            }
        }

        private void SaveBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Save Button Click]");
            militaryUnitSettingPageVM.militaryUnitSaveEvent();
            
        }
        private void homeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Home Button Click]");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        private void groupDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {

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
                StaticAttribute.Function.logCommand.infoLog("[VI.MilitaryUnitSettingPage.Logout]");
                militaryUnitSettingPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "로그아웃");
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
