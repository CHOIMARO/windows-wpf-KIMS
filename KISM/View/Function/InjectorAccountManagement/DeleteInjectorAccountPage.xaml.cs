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

namespace KISM.View.Function.InjectorAccountManagement {
    /// <summary>
    /// DeleteInjectorAccount.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeleteInjectorAccountPage : Window {
        public DeleteInjectorAccountPage() {
            InitializeComponent();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {

        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {
            GetWindow(this).Close();
            StaticAttribute.Function.tcpSendUseCase.Execute(Core.StaticAttribute.Enum.typeEnum.REQ, Core.StaticAttribute.Enum.commandEnum.UDEL, "");
            StaticAttribute.Function.loadingMessage.setMessage("주입기 내부의 계정을 삭제합니다.");
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            GetWindow(this).Close();
        }
    }
}
