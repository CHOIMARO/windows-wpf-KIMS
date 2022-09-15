using KISM.DAO.MilUnit;
using KISM.Util;
using KISM.ViewModel.Function.MilUnitInfo;
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
    /// UpdateMilUnitInfoItemPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class UpdateMilUnitInfoItemPage : Window {
        UpdateMilUnitInfoItemPageVM updateMilUnitInfoItemPageVM;
        MilUnitInfoDAO milUnitInfoItem;
        
        public UpdateMilUnitInfoItemPage(MilUnitInfoDAO milUnitInfoItem) {
            InitializeComponent();
            updateMilUnitInfoItemPageVM = new UpdateMilUnitInfoItemPageVM();
            DataContext = updateMilUnitInfoItemPageVM;

            Init(milUnitInfoItem);
            current_group_name.Text = milUnitInfoItem.Grp;
            update_group_name.Focus();
        }

        private void Init(MilUnitInfoDAO milUnitInfoItem) {
            this.milUnitInfoItem = new MilUnitInfoDAO {
                Grp = milUnitInfoItem.Grp,
                Idx = milUnitInfoItem.Idx,
                Num = milUnitInfoItem.Num,
                Rank = milUnitInfoItem.Rank,
                Stat = milUnitInfoItem.Stat,
                Timestamp = milUnitInfoItem.Timestamp,
                UniNum = milUnitInfoItem.UniNum,
                UserName = milUnitInfoItem.UserName,
            };
        }

        private void Window_Loaded(object sender, RoutedEventArgs e) {
            update_group_name.Focus();
        }

        private void Window_Unloaded(object sender, RoutedEventArgs e) {

        }
        private void YesBtn_Click(object sender, RoutedEventArgs e) {

            if(current_group_name.Text.Equals(update_group_name.Text.Trim())) {
                InformationMessage.InformationShowDialog("같은 이름으로는 변경할 수 없습니다.");
            } else {
                bool state = updateMilUnitInfoItemPageVM.DuplicateCheckMilUnitInfoItem(milUnitInfoItem.Idx, update_group_name.Text.Trim());
                if(state) {
                    GetWindow(this).Close();
                }
            }
        }

        private void NoBtn_Click(object sender, RoutedEventArgs e) {
            GetWindow(this).Close();
        }
    }
}
