using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Information;
using KISM.View.SubPage;
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
    /// SettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class SettingPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        SettingPageVM settingPageVM;
        public SettingPage() {
            InitializeComponent();
            settingPageVM = new SettingPageVM();
            DataContext = settingPageVM;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            settingPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            settingPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.SettingPage.GoBackPage]");
            settingPageVM.InsertLog(LogEnum.INFO, "세팅 페이지에서 뒤로 가기 버튼 클릭");
            NavigationService.GoBack();
        }
        private void LogListBtn_Click(object sender, RoutedEventArgs e) {
            settingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "로그 이력 관리 페이지로 이동");
            LogListPage logListPage = new LogListPage();
            NavigationService.Navigate(logListPage);
            
        }
        private void CommSetBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.SettingPage.CommSet Button Click]");
            settingPageVM.InsertLog(LogEnum.INFO, "통신 설정 페이지로 이동");
            CommSettingPage commSettingPage = new CommSettingPage();
            NavigationService.Navigate(commSettingPage);
            
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e) {
            settingPageVM.InsertLog(LogEnum.INFO, "홈 버튼 클릭");
            StaticAttribute.Function.logCommand.infoLog("[VI.SettingPage.Home Button Click]");
            NavigationService.GoBack();
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }
        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }

        public void OnNext(ReceivedFromKISDAO value) {
            
        }
    }
}
