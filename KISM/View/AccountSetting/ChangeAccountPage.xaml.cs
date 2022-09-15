using KISM.DAO;
using KISM.DAO.Account;
using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
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
    /// ChangeAccountPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ChangeAccountPage : Page, IObserver<ReceivedFromKISDAO>, IObserver<TcpIsConnectDAO> {
        ChangeAccountPageVM changeAccountPageVM;
        public ChangeAccountPage() {
            InitializeComponent();
            changeAccountPageVM = new ChangeAccountPageVM();
            DataContext = changeAccountPageVM;
            changeAccountPageVM.ShowRegisteredData();
            
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            changeAccountPageVM.SetObserver(this);
        }

        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            changeAccountPageVM.UnsetObserver(this);
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            NavigationService.GoBack();
        }

        private void RegistBtn_Click(object sender, RoutedEventArgs e) {
            bool state = false;
            StaticAttribute.Function.logCommand.infoLog("[VI.ChangeAccountPage.User Clicked Account Change Button]");
            if (CheckedAccount()) {
                if(rankGroup.SelectedItem != null && rankGroup.SelectedItem.ToString().Trim().Length > 0 &&
                        FloatingUniqueNumBox.Text.Trim().Length > 0 && FloatingDptBox.Text.Trim().Length > 0 &&
                        FloatingNameBox.Text.Trim().Length > 0) {
                
                    state = changeAccountPageVM.UpdateAccount(FloatingPasswordBox.Password, rankGroup.SelectedItem.ToString());
                    changeAccountPageVM.ShowRegisteredData();
                } else {
                    StaticAttribute.Function.logCommand.warnLog("[VI.ChangeAccountPage.Incomplete Information Exists]");
                    changeAccountPageVM.InsertLog(LogEnum.WARN, "비어있는 정보가 존재합니다.");
                    InformationMessage.InformationShowDialog("비어있는 정보가 존재합니다.\r\n 다시 한 번 확인해주세요.");
                }

            } else {
                //StaticAttribute.Function.toastMessage.showMessage(toastStateEnum.WARNING, StaticAttribute.ConstAttribute.checkedAccountInfo);
                InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.checkedAccountInfo);
            }
            if(state) {
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
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

        private void AccountDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            try {
                AccountInfoDAO accountInfoDAO = (AccountInfoDAO)adminDataGrid.CurrentCell.Item;

                rankGroup.SelectedItem = accountInfoDAO.Rank.ToString().Trim();
                changeAccountPageVM.SelectedChangeEvent(accountInfoDAO);
            }catch (InvalidCastException) {

            }
            
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {
            AccountInfoDAO accountInfoDAO = (AccountInfoDAO)adminDataGrid.CurrentCell.Item;

            rankGroup.SelectedItem = accountInfoDAO.Rank.ToString().Trim();
            changeAccountPageVM.SelectedChangeEvent(accountInfoDAO);
            
        }

        private void loginTimeExtensionBtn_Click(object sender, RoutedEventArgs e) {

        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            throw new NotImplementedException();
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
