using KISM.DAO;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
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
    /// CreateAccountPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class CreateAccountPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        CreateAccountPageVM createAccountPageVM;
        public CreateAccountPage() {
            InitializeComponent();
            createAccountPageVM = new CreateAccountPageVM();
            DataContext = createAccountPageVM;
            createAccountPageVM.ShowRegisteredData();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            createAccountPageVM.SetObserver(this);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            createAccountPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void RegistBtn_Click(object sender, RoutedEventArgs e) {
            createAccountPageVM.InsertLog(LogEnum.INFO, "사용자 생성 버튼 클릭");
            if(CheckAccountStat()) {
                if (CheckedAccount()) {
                    if (rankGroup.SelectedItem != null&& rankGroup.SelectedItem.ToString().Trim().Length > 0 &&
                        FloatingUniqueNumBox.Text.Trim().Length >0 && FloatingDptBox.Text.Trim().Length >0 &&
                        FloatingNameBox.Text.Trim().Length >0) {
                        if(createAccountPageVM.InsertAccount(FloatingPasswordBox.Password, rankGroup.SelectedItem.ToString().Trim(), 0)) {
                            goInitialPage();
                        }
                    } else {
                        StaticAttribute.Function.logCommand.warnLog("[VI.CreateAccountPage.Incomplete Information Exists]");
                        createAccountPageVM.InsertLog(LogEnum.WARN, "비어있는 정보가 존재합니다.");
                        InformationMessage.InformationShowDialog("비어있는 정보가 존재합니다.\r\n 다시 한 번 확인해주세요.");

                    }
                } else {
                    StaticAttribute.Function.logCommand.warnLog("[VI.CreateAccountPage.Incorrect Information Exists]");
                    createAccountPageVM.InsertLog(LogEnum.WARN, "비어있는 정보가 존재합니다.");
                    InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.checkedAccountInfo);
                }
            } else {
                InformationMessage.InformationShowDialog("계정은 하나만 생성될 수 있습니다.\r\n기존 계정을 삭제 후 생성해주세요.");
                createAccountPageVM.InsertLog(LogEnum.WARN, "기존 계정이 존재합니다.");
                StaticAttribute.Function.logCommand.warnLog("[VI.CreateAccountPage. An existing account exists]");
            }
        }
        private bool CheckAccountStat() {
            return createAccountPageVM.CheckAccountStat();
        }

        private void searchBtn_Click(object sender, RoutedEventArgs e) {

        }
        private bool CheckedAccount() {
            string idTxt = FloatingIDBox.Text.Trim();
            string pwTxt = FloatingPasswordBox.Password.Trim();
            string rePwTxt = FloatingReconfirmPasswordBox.Password.Trim();
            int id = -1;
            int pw = -1;
            int rePw = -1;

            bool state = false;

            
            if (idTxt.Length > 3 && idTxt.Length < 13
            && pwTxt.Length > 7 && pwTxt.Length < 16
            && rePwTxt.Length > 7 && rePwTxt.Length < 16) {
                state = int.TryParse(idTxt, out id);
                state = int.TryParse(pwTxt, out pw);
                state = int.TryParse(rePwTxt, out rePw);

                state = pw == rePw ? true : false;
            }
            
            return state;
        }
        private void goInitialPage() {
            createAccountPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "초기 화면으로 돌아가기");
            NavigationService.RemoveBackEntry();
            NavigationService.GoBack();
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

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

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            if (registUserBtn.IsMouseOver) {
                registUserBtn.BorderBrush = new SolidColorBrush(Colors.White);
                registUserBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                registUserBtn_text.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            if (!registUserBtn.IsMouseOver) {
                registUserBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                registUserBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                registUserBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
