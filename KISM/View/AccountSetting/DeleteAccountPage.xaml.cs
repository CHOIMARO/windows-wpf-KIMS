using KISM.DAO;
using KISM.DAO.Account;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.View.Function.Account;
using KISM.ViewModel.AccountSetting;
using System;
using System.Collections.Generic;
using System.Data;
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

namespace KISM.View.AccountSetting {
    /// <summary>
    /// DeleteAccountPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeleteAccountPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        DeleteAccountPageVM deleteAccountPageVM;
        public DeleteAccountPage() {
            InitializeComponent();
            deleteAccountPageVM = new DeleteAccountPageVM();
            DataContext = deleteAccountPageVM;
            deleteAccountPageVM.ShowRegisteredData();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            deleteAccountPageVM.SetObserver(this);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            deleteAccountPageVM.UnsetObserver(this);
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.DeleteAccountPage.User Clicked Back Button]");
            deleteAccountPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "사용자 계정 삭제 페이지에서 뒤로 가기 버튼 클릭");
            NavigationService.GoBack();
        }

        private void DeleteBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.DeleteAccountPage.User Clicked Account Delete Button]");
            deleteAccountPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "사용자 삭제 버튼 클릭");
            AccountInfoDAO accountInfoDAO = adminDataGrid.SelectedItem as AccountInfoDAO;
            DeleteAccountRequestPage deleteAccountRequestPage = new DeleteAccountRequestPage(accountInfoDAO);
            deleteAccountRequestPage.ShowDialog();

            if(StaticAttribute.Function.deleteAccountRequestPageState) {
                StaticAttribute.Function.deleteAccountRequestPageState = false;
                deleteAccountPageVM.DeleteAccountInfoItem(accountInfoDAO.Idx);
            }
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e) {

        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnNext(LoginExtensionDAO value) {
            if (!value.state) {
                Application.Current.Dispatcher.Invoke(DispatcherPriority.Background, new Action(() => {
                    if (NavigationService != null) {
                        NavigationService.RemoveBackEntry();
                        NavigationService.GoBack();
                    }
                }));
                StaticAttribute.Function.logCommand.infoLog("[VI.DeleteAccountPage.Logout]");
                deleteAccountPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "로그아웃");
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        public void OnNext(ReceivedFromKISDAO value) {
            
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }
    }
}
