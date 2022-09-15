using KISM.DAO;
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
    /// LogListPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogListPage : Page, IObserver<LoginExtensionDAO> {
        LogListPageVM logListPageVM;
        public LogListPage() {
            InitializeComponent();
            logListPageVM = new LogListPageVM();
            DataContext = logListPageVM;
            ListUp();
            logListPageVM.init();
            logListPageVM.setObserver(this);
        }

        private async void ListUp() {
            bool state = await logListPageVM.loadingList();
            if(state) {
                if (dataGrid.Items.Count > 0) {
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                }
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            logListPageVM.endTransmission();
            StaticAttribute.Function.createKeyRequestWindow = false;
            StaticAttribute.Function.movePageLog = false;
            logListPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "로그 이력 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e) {
            logListPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "검색 버튼 클릭");
            if (msgGroup.SelectedItem != null) {
                logListPageVM.checkUserSearch(
                datePickerStart.SelectedDate.ToString().Trim(),
                datePickerEnd.SelectedDate.ToString().Trim(),
                msgGroup.SelectedItem.ToString());
            } else {
                logListPageVM.checkUserSearch(
                datePickerStart.SelectedDate.ToString().Trim(),
                datePickerEnd.SelectedDate.ToString().Trim(),
                "");
            }

            if (dataGrid.Items.Count > 0) {
                dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
            }

        }

        private async void initializeBtn_Click(object sender, RoutedEventArgs e) {
            //logListPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "새로고침 버튼 클릭");
            bool state = await logListPageVM.loadingList();
            if (state) {
                initializeInputBox();
                if (dataGrid.Items.Count > 0) {
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                }
            }
            
            
        }
        private void initializeInputBox() {
            msgGroup.SelectedIndex = -1;
            datePickerStart.SelectedDate = null;
            datePickerEnd.SelectedDate = null;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
        }

        private void dontgotoPageBtn_Click(object sender, RoutedEventArgs e) {
            logListPageVM.closeCreateKeyRequestWindow();
        }

        private void gotoPageBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.createKeyRequestWindow = true;
            StaticAttribute.Function.createKeyReq = true;
            StaticAttribute.Function.movePage = true;
            logListPageVM.closeCreateKeyRequestWindow();
            KeyRegistrationManagementPage keyRegistrationManagementPage = new KeyRegistrationManagementPage();
            NavigationService.Navigate(keyRegistrationManagementPage);
        }

        private void LogRegNoBtn_Click(object sender, RoutedEventArgs e) {
            logListPageVM.closeLogRegRequestWindow();
        }

        private void LogRegYesBtn_Click(object sender, RoutedEventArgs e) {
            logListPageVM.closeLogRegRequestWindow();
            KeyDistributionStatusPage keyDistributionStatusPage = new KeyDistributionStatusPage();
            NavigationService.Navigate(keyDistributionStatusPage);
            //StaticAttribute.Function.createKeyRequestWindow = true;

        }

        public void OnNext(LoginExtensionDAO value) {
            if (!value.state){
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                    if (NavigationService != null) {
                        NavigationService.RemoveBackEntry();
                        NavigationService.GoBack();
                    }
                }));
                logListPageVM.endTransmission();
                StaticAttribute.Function.createKeyRequestWindow = false;
                StaticAttribute.Function.movePageLog = false;
                logListPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "로그 이력 페이지에서 뒤로 가기 버튼 클릭");
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
