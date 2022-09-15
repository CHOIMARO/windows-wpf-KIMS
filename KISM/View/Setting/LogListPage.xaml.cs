using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
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
    /// LogListPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LogListPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        LogListPageVM logListPageVM;
        public LogListPage() {
            InitializeComponent();
            logListPageVM = new LogListPageVM();
            DataContext = logListPageVM;
            logListPageVM.Init();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            logListPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            ListUp();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            logListPageVM.UnsetObserver(this);
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            logListPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "로그 이력 페이지에서 뒤로 가기 버튼 클릭");
        }
        private async void ListUp() {
            bool state = await logListPageVM.ShowRegisteredData();
            if (state) {
                if (dataGrid.Items.Count > 0) {
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                }
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e) {
            logListPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "검색 버튼 클릭");
            if (msgGroup.SelectedItem != null) {
                logListPageVM.CheckUserSearch(
                datePickerStart.SelectedDate.ToString().Trim(),
                datePickerEnd.SelectedDate.ToString().Trim(),
                msgGroup.SelectedItem.ToString());
            } else {
                logListPageVM.CheckUserSearch(
                datePickerStart.SelectedDate.ToString().Trim(),
                datePickerEnd.SelectedDate.ToString().Trim(),
                "");
            }

            if (dataGrid.Items.Count > 0) {
                dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
            }

        }

        private async void initializeBtn_Click(object sender, RoutedEventArgs e) {
            bool state = await logListPageVM.ShowRegisteredData();
            if (state) {
                InitializeInputBox();
                if (dataGrid.Items.Count > 0) {
                    dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
                }
            }
            
            
        }
        private void InitializeInputBox() {
            msgGroup.SelectedIndex = -1;
            datePickerStart.SelectedDate = null;
            datePickerEnd.SelectedDate = null;
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnNext(ReceivedFromKISDAO value) {
            
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e) {
            logListPageVM.InsertLog(LogEnum.INFO, "홈 버튼 클릭");
            StaticAttribute.Function.logCommand.infoLog("[VI.LogListPage.Home Button Click]");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }
    }
}
