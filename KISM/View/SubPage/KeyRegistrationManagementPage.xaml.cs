using KISM.DAO;
using KISM.DAO.Account;
using KISM.DAO.JSON;
using KISM.DAO.KeyRel;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.KeySending;
using KISM.View.Function.RequestFromKIS_100;
using KISM.View.Login;
using KISM.ViewModel.SubPageVM;
using System;
using System.Collections.Generic;
using System.Data;
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
using System.Windows.Threading;

namespace KISM.View.SubPage {
    /// <summary>
    /// KeyRegistrationManagementPage.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class KeyRegistrationManagementPage : Page, IObserver<List<OverExpKeyDAO>>, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        KeyRegistrationManagementPageVM keyRegistrationManagementPageVM;
        DeleteKey deleteKey = new DeleteKey();
        SelectedKey selectedKey = new SelectedKey();


        public KeyRegistrationManagementPage() {
            InitializeComponent();
            keyRegistrationManagementPageVM = new KeyRegistrationManagementPageVM();
            DataContext = keyRegistrationManagementPageVM;
            keyRegistrationManagementPageVM.ShowRegisteredData();
            keyRegistrationManagementPageVM.Init();

        }
        private void Page_Loaded(object sender, RoutedEventArgs e) {
            keyRegistrationManagementPageVM.SetObserver(this);
            StaticAttribute.Function.movePageTracker.TrackMovePageNotify(601);
            if (keyDataGrid.Items.Count > 0) {
                keyDataGrid.ScrollIntoView(keyDataGrid.Items[keyDataGrid.Items.Count - 1]);
            }
            keyRegistrationManagementPageVM.ShowOverExpData();
        }
        private void Page_Unloaded(object sender, RoutedEventArgs e) {
            keyRegistrationManagementPageVM.UnsetObserver(this);
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyRegistrationManagementPage.Back Button Click]");
            keyRegistrationManagementPageVM.InsertLog(LogEnum.INFO, "암호키 등록 관리 페이지에서 뒤로 가기 버튼 클릭");
            if (StaticAttribute.Function.movePageKeyGen) {
                NavigationService.RemoveBackEntry();
                StaticAttribute.Function.movePageKeyGen = false;
            }
            NavigationService.GoBack();
        }
        private void GenerateBtn_Click(object sender, RoutedEventArgs e) {
            keyRegistrationManagementPageVM.ShowKeyGenerationPage();
        }
        private void RegistBtn_Click(object sender, RoutedEventArgs e) {
            keyRegistrationManagementPageVM.InsertLog(LogEnum.INFO, "암호키 등록 버튼 클릭");
            if(keyInformation.Text.Trim().Length>0) {
                if (keyGroup.SelectedItem != null && keyPurpose.SelectedItem != null) {
                    string groupData = keyGroup.SelectedItem.ToString();
                    string purposeData = keyPurpose.SelectedItem.ToString();
                    string expirationData = keyExpirationDate.SelectedItem != null ? keyExpirationDate.SelectedItem.ToString() : "3개월";

                    string usingData = datePicker.SelectedDate.ToString();
                    if (usingData.Length == 0) {
                        usingData = DateTime.Now.Date.ToString();
                    }

                    if (DuplicateCheck(groupData, purposeData)) {
                        StaticAttribute.Function.logCommand.infoLog("[VI.KeyRegistrationManagementPage.Duplicate Grp/Ppose Check Success]");
                        keyRegistrationManagementPageVM.InsertLog(LogEnum.INFO, "부대 및 용도 중복 체크 완료.");
                        keyRegistrationManagementPageVM.ShowKeyRegistrationPage(groupData, purposeData, usingData, expirationData); //키 등록 요청
                        if (keyDataGrid.Items.Count > 0) {
                            keyDataGrid.ScrollIntoView(keyDataGrid.Items[keyDataGrid.Items.Count - 1]);
                        }
                    } else {
                        //StaticAttribute.Function.toastMessage.showMessage(toastStateEnum.WARNING, "부대 및 용도가 중복된 키가 존재합니다.");
                        InformationMessage.InformationShowDialog("부대 및 용도가 중복된 키가 존재합니다.");
                        StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.Duplicate Grp/Ppose Exists]");
                        keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "부대 및 용도가 중복된 키가 존재합니다.");
                    }
                } else {
                    //StaticAttribute.Function.toastMessage.showMessage(toastStateEnum.WARNING, "부대 및 용도, 만료일을 선택 바랍니다.");
                    InformationMessage.InformationShowDialog("부대 및 용도, 만료일을 선택 바랍니다.");
                    StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.User Do Not Select Grp, Ppose, ExpDate]");
                    keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "부대 및 용도, 만료일 미기입");
                }
            } else {
                InformationMessage.InformationShowDialog("암호키가 생성되어 있지 않습니다.");
                StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.Key Doesn't Exist]");
                keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "암호키가 생성되어 있지 않습니다.");
            }
            
        }

        public bool DuplicateCheck(string grp, string purpose) {
            return keyRegistrationManagementPageVM.DuplicateCheck(grp, purpose);
        }


        private void SendKeyBtn_Click(object sender, RoutedEventArgs e) {
            keyRegistrationManagementPageVM.InsertLog(LogEnum.INFO, "암호키 재주입 버튼 클릭");

            if (selectedKey.ppose == null) {
                InformationMessage.InformationShowDialog("출고할 키를 선택해주세요.");
                StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.User Do Not Select Sending Key]");
                keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "출고할 키를 선택하지 않았습니다.");
            } else if (selectedKey.ppose !=null) {
                if(StaticAttribute.Function.authState) {
                    if(selectedKey.expDate > DateTime.Now.Date) {
                        keyRegistrationManagementPageVM.ShowKeySendingPage(selectedKey);
                    } else {
                        StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.Keys Past The Expiration Date Cannot Be Sent]");
                        keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "만료일이 지난 키는 출고할 수 없습니다.");
                        InformationMessage.InformationShowDialog("만료일이 지난 키는 출고할 수 없습니다.");
                    }
                } else {
                    StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.Mutual Authentication with The Injector Has Not Been Verified]");
                    InformationMessage.InformationShowDialog("주입기와의 상호 인증이 완료되지 않았습니다.");
                    keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "주입기와의 상호 인증이 완료되지 않았습니다.");
                }
            }
        }
        
        private void DeleteBtn_Click(object sender, RoutedEventArgs e) {
            StaticAttribute.Function.logCommand.infoLog("[VI.KeyRegistrationManagementPage.Delete Key Button Click]");
            keyRegistrationManagementPageVM.InsertLog(LogEnum.INFO, "암호키 삭제 버튼 클릭");
            if (deleteKey.dpt != null && deleteKey.ppose != null) {
                keyRegistrationManagementPageVM.ShowKeyDeletionPage(deleteKey);
            } else {
                InformationMessage.InformationShowDialog("삭제할 키를 선택해주세요.");
                StaticAttribute.Function.logCommand.warnLog("[VI.KeyRegistrationManagementPage.User Do Not Select A Key To Delete]");
                keyRegistrationManagementPageVM.InsertLog(LogEnum.WARN, "삭제할 키를 선택하지 않았습니다.");
            }
        }

        private DeleteKey SetDeleteKeyObject(KeyRelDAO keyRelDAO) {
            return new DeleteKey {
                timestamp = DateTime.Parse(keyRelDAO.Timestamp),
                dpt = keyRelDAO.Dpt,
                ppose = keyRelDAO.Ppose,
                stat = "DEL",
                idx = keyRelDAO.Idx,
                wkey = keyRelDAO.Wkey,
            };
        }
        private SelectedKey SetSelectedKeyObject(KeyRelDAO keyRelDAO) {
            return new SelectedKey {
                num = keyRelDAO.Num,
                idx = keyRelDAO.Idx,
                timestamp = DateTime.Parse(keyRelDAO.Timestamp),
                stDate = DateTime.Parse(keyRelDAO.StDate),
                expDate = DateTime.Parse(keyRelDAO.ExpDate),
                dpt = keyRelDAO.Dpt,
                ppose = keyRelDAO.Ppose,
                wkey = keyRelDAO.Wkey,
                stat = keyRelDAO.Stat.Equals("키 등록 완료") ? "REG" :
                               keyRelDAO.Stat.Equals("키 재배포 완료") ? "WAIT" :
                               keyRelDAO.Stat.Equals("키 배포 완료") ? "DT" : "DEL",
            };
        }

        private void keyDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e) {
            ClickEvent();

        }

        private void RadioButton_Click(object sender, RoutedEventArgs e) {
            ClickEvent();
        }
        private void ClickEvent() {
            try {
                KeyRelDAO keyRelDAO = (KeyRelDAO)keyDataGrid.CurrentCell.Item;

                selectedKey = SetSelectedKeyObject(keyRelDAO);
                deleteKey = (SetDeleteKeyObject(keyRelDAO));
                keyRegistrationManagementPageVM.InitializeKeyInfo();
                keyGroup.SelectedIndex = -1;
                keyPurpose.SelectedIndex = -1;
                keyExpirationDate.SelectedIndex = -1;
                keyRegistrationManagementPageVM.changeOpacity();
                datePicker.SelectedDate = null;
                keyInformation.Text = "";
            }catch (InvalidCastException) {

            }
        }
        private void initializeKeyInfo() {
            keyRegistrationManagementPageVM.InitializeKeyInfo();
            keyGroup.SelectedIndex = -1;
            keyPurpose.SelectedIndex = -1;
            keyExpirationDate.SelectedIndex = -1;
        }
        public void OnNext(List<OverExpKeyDAO> value) {
            Application.Current.Dispatcher.BeginInvoke(DispatcherPriority.Background, new Action(() =>
            {
                for (int i = 0; i < value.Count; i++) {
                    DataGridRow row = (DataGridRow)keyDataGrid.ItemContainerGenerator.ContainerFromIndex(value[i].num);
                    if (row != null) {
                        if(value[i].stat.Equals("Red")) {
                            //row.Background = Brushes.Red;
                            row.Foreground = Brushes.Red;
                        } else if (value[i].stat.Equals("Orange")) {
                            //row.Background = Brushes.Orange;
                            row.Foreground = Brushes.Orange;
                        } else if (value[i].stat.Equals("Yellow")) {
                            //row.Background = Brushes.Yellow;
                            row.Foreground = (SolidColorBrush)new BrushConverter().ConvertFrom("#FFFFD600");
                        } else if (value[i].stat.Equals("Green")) {
                            //row.Background = Brushes.Green;
                            row.Foreground = Brushes.Green;
                        }
                    }
                }
            }));
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            
        }


        public void OnNext(TcpIsConnectDAO value) {
            
        }

        public void OnNext(ReceivedFromKISDAO value) {
            Application.Current.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate {
                switch (value.type) {
                    case typeEnum.RES:
                        switch (value.cmd) {
                            case commandEnum.REK:
                                if(value.msg.stat.Equals("Y")) {
                                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                                    InformationMessage.InformationShowDialog("암호키 출고에 성공했습니다.");
                                }
                                break;
                            case commandEnum.KDEL:
                                if (value.msg.stat.Equals("Y")) {
                                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                                    InformationMessage.InformationShowDialog("암호키 삭제에 성공했습니다.");

                                } else if (value.msg.stat.Equals("M")) { //키 해시값이 다를 때, 즉 다른 키주입기 또는 다른 키 서버와 연결해서 삭제하려고 할 때 발생
                                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                                    InformationMessage.InformationShowDialog("키 해시 불일치 에러가 발생했습니다.\r\n" +
                                                                                                     "      관리자에게 문의해주세요.");
                                } else if (value.msg.stat.Equals("N")) {
                                    StaticAttribute.Function.loadingMessage.loadingViewClose();
                                    InformationMessage.InformationShowDialog("        암호키 삭제에는 성공했으나,\r\n " +
                                                                                                     "주입기 내부의 키는 삭제하지 못했습니다.");
                                }
                                break;
                        }
                        break;
                    case typeEnum.REQ:
                        switch (value.cmd) {
                            case commandEnum.GEN:
                                keyRegistrationManagementPageVM.RequestKeyGen();
                                break;
                            case commandEnum.RLOG:
                                RequestHistorySavePage requestHistorySavePage = new RequestHistorySavePage();
                                requestHistorySavePage.ShowDialog();

                                if (StaticAttribute.Function.movePageHistorySave) { //이력 등록 페이지 이동
                                    KeyDistributionStatusPage keyDistributionStatusPage = new KeyDistributionStatusPage();
                                    NavigationService.Navigate(keyDistributionStatusPage);
                                    
                                    StaticAttribute.Function.logCommand.infoLog("[VM.MainPage.Go To KeyDistributionStatusPage]");
                                    keyRegistrationManagementPageVM.InsertLog(StaticAttribute.Enum.LogEnum.INFO, "암호키 이력 등록 페이지로 이동");
                                }
                                break;

                        }
                        break;
                    case typeEnum.END:
                        break;
                }
            }));
            
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e) {
            if(generateKeyBtn.IsMouseOver) {
                generateKeyBtn.BorderBrush = new SolidColorBrush(Colors.White);
                generateKeyBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                generateKeyBtn_text.Foreground = new SolidColorBrush(Colors.White);
            }else if (registKeyBtn.IsMouseOver) {
                registKeyBtn.BorderBrush = new SolidColorBrush(Colors.White);
                registKeyBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                registKeyBtn_text.Foreground = new SolidColorBrush(Colors.White);
            } else if (sendKeyBtn.IsMouseOver) {
                sendKeyBtn.BorderBrush = new SolidColorBrush(Colors.White);
                sendKeyBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                sendKeyBtn_text.Foreground = new SolidColorBrush(Colors.White);
            } else if (deleteKeyBtn.IsMouseOver) {
                deleteKeyBtn.BorderBrush = new SolidColorBrush(Colors.White);
                deleteKeyBtn_icon.Foreground = new SolidColorBrush(Colors.White);
                deleteKeyBtn_text.Foreground = new SolidColorBrush(Colors.White);
            }
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e) {
            if (!generateKeyBtn.IsMouseOver) {
                generateKeyBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                generateKeyBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                generateKeyBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            } 
            if (!registKeyBtn.IsMouseOver) {
                registKeyBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                registKeyBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                registKeyBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            } 
            if (!sendKeyBtn.IsMouseOver) {
                sendKeyBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                sendKeyBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                sendKeyBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            } 
            if (!deleteKeyBtn.IsMouseOver) {
                deleteKeyBtn.BorderBrush = new SolidColorBrush(Colors.Black);
                deleteKeyBtn_icon.Foreground = new SolidColorBrush(Colors.Black);
                deleteKeyBtn_text.Foreground = new SolidColorBrush(Colors.Black);
            }
        }
    }
}
