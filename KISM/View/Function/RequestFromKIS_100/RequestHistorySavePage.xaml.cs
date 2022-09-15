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

namespace KISM.View.Function.RequestFromKIS_100 {
    /// <summary>
    /// RequestHistorySavePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RequestHistorySavePage : Window {
        public RequestHistorySavePage() {
            InitializeComponent();
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageHistorySave = true;
            GetWindow(this).Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageHistorySave = false;
            GetWindow(this).Close();
        }
    }
}
