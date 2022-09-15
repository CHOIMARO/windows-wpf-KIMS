using KISM.DAO.Ppose;
using KISM.Util;
using KISM.ViewModel.Function.PposeInfo;
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

namespace KISM.View.Function.PposeInfo {
    /// <summary>
    /// UpdatePposeInfoItemPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UpdatePposeInfoItemPage : Window {
        UpdatePposeInfoItemPageVM updatePposeInfoItemPageVM;
        PposeInfoDAO pposeInfoItem;
        public UpdatePposeInfoItemPage(PposeInfoDAO pposeInfoItem) {
            InitializeComponent();
            updatePposeInfoItemPageVM = new UpdatePposeInfoItemPageVM();
            DataContext = updatePposeInfoItemPageVM;

            Init(pposeInfoItem);
            current_purpose_name.Text = pposeInfoItem.Ppose;
            update_purpose_name.Focus();
        }

        private void Init(PposeInfoDAO pposeInfoItem) {
            this.pposeInfoItem = pposeInfoItem;
        }

        private void YesBtn_Click(object sender, RoutedEventArgs e) {

            if (current_purpose_name.Text.Equals(update_purpose_name.Text.Trim())) {
                InformationMessage.InformationShowDialog("같은 이름으로는 변경할 수 없습니다.");
            } else {
                bool state = updatePposeInfoItemPageVM.DuplicateCheckPposeInfoItem(pposeInfoItem.Idx, update_purpose_name.Text.Trim());
                if (state) {
                    GetWindow(this).Close();
                }
            }
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            GetWindow(this).Close();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {

        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {

        }
    }
}
