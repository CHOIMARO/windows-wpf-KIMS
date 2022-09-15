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

namespace KISM.View.Function.HistoryInitialize {
    /// <summary>
    /// HistoryInitializePage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class HistoryInitializePage : Window {
        public HistoryInitializePage() {
            InitializeComponent();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {

        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.historyInitializePageState = true;
            GetWindow(this).Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.historyInitializePageState = false;
            GetWindow(this).Close();
        }
    }
}
