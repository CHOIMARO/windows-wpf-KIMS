using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.ViewModel.SubPageDataGridVM;
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

namespace KISM.View.SubPageDataGrid {
    /// <summary>
    /// KeyDistributionRecordPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyDistributionRecordPage : Grid, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        KeyDistributionRecordPageVM keyDistributionRecordPageVM;
        public KeyDistributionRecordPage() {
            InitializeComponent();
            keyDistributionRecordPageVM = new KeyDistributionRecordPageVM();
            DataContext = keyDistributionRecordPageVM;

        }
        private void Grid_Loaded(object sender, RoutedEventArgs e) {
            keyDistributionRecordPageVM.SetMdItems();
            keyDistributionRecordPageVM.SetGrp();
            keyDistributionRecordPageVM.SetPpose();
            keyDistributionRecordPageVM.SetObserver(this);
            ListUp();
        }
        private void Grid_Unloaded(object sender, RoutedEventArgs e) {
            keyDistributionRecordPageVM.UnsetObserver(this);
        }

        private async void ListUp() {
            //bool state = await keyDistributionRecordPageVM.ShowRegisteredData();
            keyDistributionRecordPageVM.ShowRegisteredData();
            //if (state) {
                FocusingLastLine();
                //bool state2 = await Delay(2000);
                //if (state2) {
                //    FocusingLastLine();
                //}
            //}
        }

        private async Task<bool> Delay(int v) {
            await Task.Delay(v);
            return true;
        }
        public void FocusingLastLine() {
            if (dataGrid.Items.Count > 0) {
                dataGrid.ScrollIntoView(dataGrid.Items[dataGrid.Items.Count - 1]);
            }
        }

        private void SearchBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyDistributionRecordPage.Search Button Click]");
            keyDistributionRecordPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "검색 버튼 클릭");
            if (keyGroup.SelectedItem != null) {
                if(mdComboBox.SelectedItem != null) {
                    keyDistributionRecordPageVM.CheckUserSearch(
                    datePickerStart.SelectedDate.ToString().Trim(),
                    datePickerEnd.SelectedDate.ToString().Trim(),
                    mdComboBox.SelectedItem.ToString().Trim(),
                    keyGroup.SelectedItem.ToString());
                } else {
                    keyDistributionRecordPageVM.CheckUserSearch(
                    datePickerStart.SelectedDate.ToString().Trim(),
                    datePickerEnd.SelectedDate.ToString().Trim(),
                    "",
                    keyGroup.SelectedItem.ToString());
                }
            } else {
                if (mdComboBox.SelectedItem != null) {
                    keyDistributionRecordPageVM.CheckUserSearch(
                    datePickerStart.SelectedDate.ToString().Trim(),
                    datePickerEnd.SelectedDate.ToString().Trim(),
                    mdComboBox.SelectedItem.ToString().Trim(),
                    "");
                } else {
                    keyDistributionRecordPageVM.CheckUserSearch(
                    datePickerStart.SelectedDate.ToString().Trim(),
                    datePickerEnd.SelectedDate.ToString().Trim(),
                    "",
                    "");
                }
            }
        }

        private void InitializeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyDistributionRecordPage.Initalize Button Click]");
            keyDistributionRecordPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "이력 초기화 버튼 클릭");
            InitializeInputBox();
            ListUp();
            

        }
        private void InitializeInputBox() {
            mdComboBox.SelectedIndex = -1;
            keyGroup.SelectedIndex = -1;
            datePickerStart.SelectedDate = null;
            datePickerEnd.SelectedDate = null;
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }
        public void OnNext(ReceivedFromKISDAO value) {
            
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }
    }
}
