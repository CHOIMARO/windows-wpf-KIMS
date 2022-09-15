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
    /// RequestKeyGenerationWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class RequestKeyGenerationPage : Window {
        public RequestKeyGenerationPage() {
            InitializeComponent();
        }

        private void noBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageKeyGen = false;
            GetWindow(this).Close();
        }

        private void yesBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.movePageKeyGen = true;
            GetWindow(this).Close();
        }
    }
}
