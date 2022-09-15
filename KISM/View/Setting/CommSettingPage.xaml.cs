using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
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
    /// CommSettingPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CommSettingPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        CommSettingPageVM commSettingPageVM;
        public CommSettingPage() {
            InitializeComponent();
            commSettingPageVM = new CommSettingPageVM();
            DataContext = commSettingPageVM;
            commSettingPageVM.Init();
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            commSettingPageVM.InitPort();
            commSettingPageVM.InitSettingInfo();
            commSettingPageVM.SetObserver(this);
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            commSettingPageVM.UnsetObserver(this);
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
            StaticAttribute.Function.logCommand.infoLog("[VI.CommSettingPage.GoBackPage]");
            commSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "통신 설정 페이지에서 뒤로 가기 버튼 클릭");
        }

        private void OkBtn_Click(object sender, RoutedEventArgs e) {
            commSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "통신 설정 버튼 클릭");

            //TCP 설정
            commSettingPageVM.SaveBtnEvent();
        }

        private void HomeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.CommsettingPage.Home Button Click]");
            commSettingPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "홈 버튼 클릭");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
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
    }
}
