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
    /// ManagerRegistKIS100Page.xaml에 대한 상호 작용 논리
    /// </summary>
    public partial class ManagerRegistKIS100Page : Grid, IObserver<TcpIsConnectDAO>, IObserver<ReceivedFromKISDAO> {
        ManagerRegistKIS100PageVM managerRegistKIS100PageVM;
        public ManagerRegistKIS100Page() {
            InitializeComponent();
            managerRegistKIS100PageVM = new ManagerRegistKIS100PageVM();
            DataContext = managerRegistKIS100PageVM;
            managerRegistKIS100PageVM.CheckUser();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e) {
            managerRegistKIS100PageVM.SetObserver(this);
        }
        private void Grid_Unloaded(object sender, RoutedEventArgs e) {
            managerRegistKIS100PageVM.UnsetObserver(this);
        }

        private void registBtn_Click(object sender, RoutedEventArgs e) {
            managerRegistKIS100PageVM.InsertLog(LogEnum.INFO, "주입기 계정 설정 버튼 클릭.");
            if (StaticAttribute.Function.authState) {
                if (checkedAccount()) {
                    managerRegistKIS100PageVM.InsertLog(LogEnum.INFO, "주입기 계정 가입 조건에 맞는 지 확인 완료.");
                    managerRegistKIS100PageVM.RegistUserReq(FloatingIDBox.Text.Trim(), FloatingPasswordBox.Password.Trim()); // 사용자 등록 메시지 송신
                    initializeInputText();
                } else {
                    InformationMessage.InformationShowDialog("ID/PW를 확인 바랍니다.");
                    managerRegistKIS100PageVM.InsertLog(LogEnum.WARN, "ID/PW가 조건에 맞지 않음.");
                }
            } else {
                InformationMessage.InformationShowDialog("주입기와의 상호 인증이 완료되지 않았습니다.");
                managerRegistKIS100PageVM.InsertLog(LogEnum.WARN, "주입기와의 상호 인증이 완료되지 않았습니다.");
            }
        }
        private void initializeInputText() {
            FloatingIDBox.Text = "";
            FloatingPasswordBox.Password = "";
            FloatingReconfirmPasswordBox.Password = "";
        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e) {
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

            if (idTxt.Length == 4
                && pwTxt.Length > 3 && pwTxt.Length < 9
                && rePwTxt.Length > 3 && rePwTxt.Length < 9) {
                state = int.TryParse(idTxt, out id);
                state = int.TryParse(pwTxt, out pw);
                state = int.TryParse(rePwTxt, out rePw);

                state = pw == rePw ? true : false;
            }
            return state;
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
                                managerRegistKIS100PageVM.CheckUser();
                            } else {
                                StaticAttribute.Function.accountState = false;
                                managerRegistKIS100PageVM.CheckUser();
                            }
                            break;
                        case commandEnum.UDEL:
                            if (value.msg.stat.Equals("Y")) {
                                StaticAttribute.Function.accountState = false;
                                managerRegistKIS100PageVM.CheckUser();
                            }
                            break;
                        case commandEnum.UCK:
                            managerRegistKIS100PageVM.CheckUser();
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
