using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.ViewModel.AccountSetting;
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

namespace KISM.View.AccountSetting {
    /// <summary>
    /// AccountSettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class AccountSettingPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        AccountSettingPageVM accountSettingPageVM;
        public AccountSettingPage() {
            InitializeComponent();
            accountSettingPageVM = new AccountSettingPageVM();
            DataContext = accountSettingPageVM;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            accountSettingPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            accountSettingPageVM.UnsetObserver(this);
        }

        private void userAccountCreateBtn_Click(object sender, RoutedEventArgs e) {
            accountSettingPageVM.InsertLog(LogEnum.INFO, "사용자 계정 생성 페이지로 이동");
            CreateAccountPage createAccountPage = new CreateAccountPage();
            NavigationService.Navigate(createAccountPage);
        }

        private void userAccountChangeBtn_Click(object sender, RoutedEventArgs e) {
            accountSettingPageVM.InsertLog(LogEnum.INFO, "사용자 계정 변경 페이지로 이동");
            ChangeAccountPage changeAccountPage = new ChangeAccountPage();
            NavigationService.Navigate(changeAccountPage);
        }

        private void userAccountDeleteBtn_Click(object sender, RoutedEventArgs e) {
            accountSettingPageVM.InsertLog(LogEnum.INFO, "사용자 계정 삭제 페이지로 이동");
            DeleteAccountPage deleteAccountPage = new DeleteAccountPage();
            NavigationService.Navigate(deleteAccountPage);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            accountSettingPageVM.InsertLog(LogEnum.INFO, "로그아웃 버튼 클릭");
            accountSettingPageVM.Logout();
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }

        public void OnNext(ReceivedFromKISDAO value) {
            
        }
    }
}
