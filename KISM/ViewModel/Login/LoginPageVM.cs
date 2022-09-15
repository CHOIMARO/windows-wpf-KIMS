using KISM.DAO.Account;
using KISM.StaticAttribute.Enum;
using KISM.Util;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KISM.ViewModel.Login {
    class LoginPageVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal bool CheckedAccount(string id, string password) {
            account userInfo = new account {
                uid = id,
                upw = StaticAttribute.Function.encryptionCommand.dataHashing(id, password)
            };
            var userInfoItem = StaticAttribute.Function.checkedAccountInfoItemUseCase.Execute(userInfo);
            StaticAttribute.ConstAttribute.userInfo = new userInfo {
                dpt = userInfoItem.dpt,
                email = userInfoItem.email,
                pmit = userInfoItem.pmit,
                rank = userInfoItem.rank,
                stat = userInfoItem.stat,
                tel = userInfoItem.tel,
                timestamp = userInfoItem.timestamp,
                uid = userInfoItem.uid,
                uname = userInfoItem.uname,
                uninum = userInfoItem.uninum
            };


            if(StaticAttribute.ConstAttribute.userInfo.stat) {
                StaticAttribute.Function.logCommand.infoLog("[VM.Login] Admin Login Success");
                InsertLog(StaticAttribute.Enum.LogEnum.INFO, "관리자 로그인 성공");
                return true;
            } else {
                InformationMessage.InformationShowDialog(StaticAttribute.ConstAttribute.loginFailed);
                StaticAttribute.Function.logCommand.infoLog("[VM.Login] Admin Login Fail");
                InsertLog(StaticAttribute.Enum.LogEnum.WARN, "관리자 로그인 실패");
                return false;
            }
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }
    }
}
