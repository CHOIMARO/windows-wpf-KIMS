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

namespace KISM.View.Information {
    /// <summary>
    /// InformationWindow.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class InformationWindow : Window {
        public InformationWindow() {
            InitializeComponent();
            this.PreviewKeyUp += InformationWindow_PreviewKeyUp;
        }
        private void InformationWindow_PreviewKeyUp(object sender, KeyEventArgs e) {
            if (e.Key == Key.Enter) {
                GetWindow(this).Close();
            }
        }
        private void Button_Click(object sender, RoutedEventArgs e) {
            GetWindow(this).Close();
        }

        public void informationMessageSetting(string message) {
            informationMessage.Text = message;
        }
        public void settingWindow() {
            this.Width = 800;
            this.Height = 300;
        }
    }
}
