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

namespace KISM.View.Function.MilUnitInfo {
    /// <summary>
    /// DeleteMilUnitInfoItemPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class DeleteMilUnitInfoItemPage : Window {
        public DeleteMilUnitInfoItemPage(string grp) {
            InitializeComponent();
            group_name.Text = grp;
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.deleteMilUnitInfoItemPageState = true;
            GetWindow(this).Close();
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.deleteMilUnitInfoItemPageState = false;
            GetWindow(this).Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {

        }
    }
}
