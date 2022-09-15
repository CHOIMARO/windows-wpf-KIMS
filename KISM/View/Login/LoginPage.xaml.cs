using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.AccountSetting;
using KISM.View.SubPage;
using KISM.ViewModel.Login;
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

namespace KISM.View.Login {
    /// <summary>
    /// LoginPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class LoginPage : Page {
        LoginPageVM loginPageVM;
        public LoginPage() {
            InitializeComponent();
            loginPageVM = new LoginPageVM();
            DataContext = loginPageVM;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            Init();
        }

        private void Init() {
            string latestUID = Config.Util.ConfigCommand.getAppConfig("UID") != null ? Config.Util.ConfigCommand.getAppConfig("UID") : "";
            IDTxt.Text = latestUID;
            if (latestUID.Length > 0) {
                rememberIdCheck.IsChecked = true;
            } else {
                rememberIdCheck.IsChecked = false;
            }
            if (IDTxt.Text.Length > 0) {
                PWDTxt.Focus();
            } else {
                IDTxt.Focus();
            }
        }

        private void AccountBtn_Click(object sender, RoutedEventArgs e) {
            if (CheckedAuth()) {
                if ((bool)rememberIdCheck.IsChecked) {
                    Config.Util.ConfigCommand.SetAppConfig("UID", IDTxt.Text);
                } else {
                    Config.Util.ConfigCommand.SetAppConfig("UID", "");
                }
                AccountSettingPage accountSettingPage = new AccountSettingPage();
                NavigationService.Navigate(accountSettingPage);
            }
        }

        private void LoginBtn_Click(object sender, RoutedEventArgs e) {
            if (CheckedAuth()) {
                if((bool)rememberIdCheck.IsChecked) {
                    Config.Util.ConfigCommand.SetAppConfig("UID", IDTxt.Text);
                } else {
                    Config.Util.ConfigCommand.SetAppConfig("UID", "");
                }
                
                MainPage mainPage = new MainPage();
                NavigationService.Navigate(mainPage);
            }
        }

        private bool CheckedAuth() {
            return loginPageVM.CheckedAccount(IDTxt.Text, PWDTxt.Password);
        }

        private void IDTxt_PreviewKeyUp(object sender, KeyEventArgs e) {
            if(e.Key == Key.Enter) {
                PWDTxt.Focus();
            }
        }

        private void PWDTxt_PreviewKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                LoginBtn_Click(sender, e);
            }
        }
    }
}
