using KISM.DAO.Account;
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
using System.Windows.Shapes;

namespace KISM.View.Function.Account {
    /// <summary>
    /// DeleteAccountPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeleteAccountRequestPage : Window {
        public DeleteAccountRequestPage(AccountInfoDAO accountInfoDAO) {
            InitializeComponent();
            user_id.Text = accountInfoDAO.UserId;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {

        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.deleteAccountRequestPageState = true;
            GetWindow(this).Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.deleteAccountRequestPageState = false;
            GetWindow(this).Close();
        }
    }
}
