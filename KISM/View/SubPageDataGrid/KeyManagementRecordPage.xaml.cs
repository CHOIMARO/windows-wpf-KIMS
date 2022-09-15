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
    /// KeyManagementRecordPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyManagementRecordPage : Grid, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        KeyManagementRecordPageVM keyManagementRecordPageVM;
        public KeyManagementRecordPage() {
            InitializeComponent();
            keyManagementRecordPageVM = new KeyManagementRecordPageVM();
            DataContext = keyManagementRecordPageVM;
            keyManagementRecordPageVM.SetGrp();
            keyManagementRecordPageVM.SetPpose();
        }
        private void Grid_Loaded(object sender, RoutedEventArgs e) {
            keyManagementRecordPageVM.SetObserver(this);
            ListUp();
        }
        private void Grid_Unloaded(object sender, RoutedEventArgs e) {
            keyManagementRecordPageVM.UnsetObserver(this);
        }

        private async void ListUp() {
            //bool state = await keyManagementRecordPageVM.ShowRegisteredData();
            //if (state) {
            //    FocusingLastLine();
            //    //bool state = await Delay(5000);
            //    //if(state) {

            //    //}
            //}
            keyManagementRecordPageVM.ShowRegisteredData();
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
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyManagementRecordPage.Search Button Click]");
            keyManagementRecordPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "검색 버튼 클릭");
            if (keyGroup.SelectedItem != null) {
                if(grpComboBox.SelectedItem == null) {
                    if(pposeComboBox.SelectedItem == null) {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        "",
                        "",
                        keyGroup.SelectedItem.ToString());
                    } else {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        "",
                        pposeComboBox.SelectedItem.ToString().Trim(),
                        keyGroup.SelectedItem.ToString());
                    }
                }else {
                    if (pposeComboBox.SelectedItem == null) {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        grpComboBox.SelectedItem.ToString().Trim(),
                        "",
                        keyGroup.SelectedItem.ToString());
                    } else {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        grpComboBox.SelectedItem.ToString().Trim(),
                        pposeComboBox.SelectedItem.ToString().Trim(),
                        keyGroup.SelectedItem.ToString());
                    }
                }
            } else if(keyGroup.SelectedItem == null ) {
                if (grpComboBox.SelectedItem == null) {
                    if (pposeComboBox.SelectedItem == null) {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        "",
                        "",
                        "");
                    } else {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        "",
                        pposeComboBox.SelectedItem.ToString().Trim(),
                        "");
                    }
                } else {
                    if (pposeComboBox.SelectedItem == null) {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        grpComboBox.SelectedItem.ToString().Trim(),
                        "",
                        "");
                    } else {
                        keyManagementRecordPageVM.CheckUserSearch(
                        datePickerStart.SelectedDate.ToString().Trim(),
                        datePickerEnd.SelectedDate.ToString().Trim(),
                        grpComboBox.SelectedItem.ToString().Trim(),
                        pposeComboBox.SelectedItem.ToString().Trim(),
                        "");
                    }
                }
            }
            FocusingLastLine();
        }

        private void InitializeBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyManagementRecordPage.Initialize Button Click]");
            //keyManagementRecordPageVM.insertLog(StaticAttribute.Enum.LogEnum.INFO, "새로고침 버튼 클릭");
            InitializeInputBox();
            ListUp();
            

        }
        private void InitializeInputBox() {
            grpComboBox.SelectedIndex = -1;
            pposeComboBox.SelectedIndex = -1;
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
