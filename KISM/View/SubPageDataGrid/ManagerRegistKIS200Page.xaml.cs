using KISM.DAO.JSON;
using KISM.DAO.TCP;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using KISM.View.Function.InjectorAccountManagement;
using KISM.ViewModel.SubPageDataGridVM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    /// ManagerRegistKIS200Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManagerRegistKIS200Page : Grid, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        ManagerRegistKIS200PageVM managerRegistKIS200PageVM;
        public ManagerRegistKIS200Page() {
            InitializeComponent();
            managerRegistKIS200PageVM = new ManagerRegistKIS200PageVM();
            DataContext = managerRegistKIS200PageVM;
            managerRegistKIS200PageVM.CheckUser();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e) {
            managerRegistKIS200PageVM.SetObserver(this);
        }
        private void Grid_Unloaded(object sender, RoutedEventArgs e) {
            managerRegistKIS200PageVM.UnsetObserver(this);
        }

        private void RegistBtn_Click(object sender, RoutedEventArgs e) {
            managerRegistKIS200PageVM.InsertLog(LogEnum.INFO, "주입기 계정 설정 버튼 클릭.");
            if (StaticAttribute.Function.authState) {
                if (checkedAccount()) {
                    managerRegistKIS200PageVM.InsertLog(LogEnum.INFO, "주입기 계정 가입 조건에 맞는 지 확인 완료.");
                    managerRegistKIS200PageVM.RegistUserReq(FloatingIDBox.Text.Trim(), FloatingPasswordBox.Password.Trim()); // 사용자 등록 메시지 송신
                    initializeInputText();
                } else {
                    InformationMessage.InformationShowDialog("ID/PW를 확인 바랍니다.");
                    managerRegistKIS200PageVM.InsertLog(LogEnum.WARN, "ID/PW가 조건에 맞지 않음.");
                }
            } else {
                InformationMessage.InformationShowDialog("주입기와의 상호 인증이 완료되지 않았습니다.");
                managerRegistKIS200PageVM.InsertLog(LogEnum.WARN, "주입기와의 상호 인증이 완료되지 않았습니다.");
            }
        }
        private void initializeInputText() {
            FloatingIDBox.Text = "";
            FloatingPasswordBox.Password = "";
            FloatingReconfirmPasswordBox.Password = "";
        }


        private void DeleteBtn_Click(object sender, RoutedEventArgs e) {
            DeleteInjectorAccountPage deleteInjectorAccountPage = new DeleteInjectorAccountPage();
            deleteInjectorAccountPage.ShowDialog();
        }
        private bool checkedAccount() {
            string idTxt = FloatingIDBox.Text.Trim();
            string pwTxt = FloatingPasswordBox.Password.Trim();
            string rePwTxt = FloatingReconfirmPasswordBox.Password.Trim();
            int id = -1;
            int pw = -1;
            int rePw = -1;

            bool state = false;
            if(idCheck(idTxt) && passwordCheck(pwTxt)) {
                if (idTxt.Length >= 6 && idTxt.Length <= 12
                && pwTxt.Length >= 8 && pwTxt.Length <= 20
                && rePwTxt.Length >= 8 && rePwTxt.Length <= 20
                && pwTxt.Equals(rePwTxt)) {
                    state = int.TryParse(idTxt, out id);
                    state = int.TryParse(pwTxt, out pw);
                    state = int.TryParse(rePwTxt, out rePw);

                    state = pw == rePw ? true : false;
                }
            } else {
                InformationMessage.InformationShowDialog("ID/PW 조건에 맞지 않는 정보가 존재합니다.");
            }

            return state;
            
        }
        private bool idCheck(string value) {
            return Regex.IsMatch(value, @"^[0-9a-zA-Z]");
        }
        private bool passwordCheck(string value) {
            return Regex.IsMatch(value, @"[a-zA-Z0-9~`!@#$%^&*()_\-+={}[\]|\\;:'""<>,.?/]");
        }

        public void OnNext(TcpIsConnectDAO value) {
            
        }
        public void OnNext(ReceivedFromKISDAO value) {
            switch (value.type) {
                case typeEnum.RES:
                    switch (value.cmd) {
                        case commandEnum.UREG:
                            if (value.msg.stat.Equals("Y")) {
                                StaticAttribute.Function.accountState = true;
                                managerRegistKIS200PageVM.CheckUser();
                            } else {
                                StaticAttribute.Function.accountState = false;
                                managerRegistKIS200PageVM.CheckUser();
                            }
                            break;
                        case commandEnum.UDEL:
                            if(value.msg.stat.Equals("Y")) {
                                StaticAttribute.Function.accountState = false;
                                managerRegistKIS200PageVM.CheckUser();
                            }
                            break;
                        case commandEnum.UCK:
                            managerRegistKIS200PageVM.CheckUser();
                            break;
                    }
                    break;
                case typeEnum.REQ:
                    switch (value.cmd) {
                        
                    }
                    break;
                case typeEnum.END:
                    break;
            }
        }

        public void OnError(Exception error) {
            throw new NotImplementedException();
        }

        public void OnCompleted() {
            throw new NotImplementedException();
        }
    }
}
