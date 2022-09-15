using KISM.StaticAttribute.Enum;
using KISM.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KISM.ViewModel.Function.PposeInfo {
    public class UpdatePposeInfoItemPageVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal bool DuplicateCheckPposeInfoItem(int idx, string updatePpose) {
            var pposeInfoAll = StaticAttribute.Function.selectPposeInfoAllUseCase.Execute();
            foreach (var pposeInfoItem in pposeInfoAll) {
                if (pposeInfoItem.ppose.Equals(updatePpose) && pposeInfoItem.stat.Equals("A")) {
                    InformationMessage.InformationShowDialog("중복된 용도가 존재합니다.");
                    InsertLog(LogEnum.INFO, "중복된 용도가 존재합니다.");
                    return false;
                }
            }

            return UpdatePposeInfoItem(idx, updatePpose);
        }
        internal bool UpdatePposeInfoItem(int idx, string updatePpose) {

            bool state = StaticAttribute.Function.updatePposeInfoItemUseCase.Execute(idx, updatePpose);
            if (state) {
                InformationMessage.InformationShowDialog("용도 변경이 완료되었습니다.");
                InsertLog(LogEnum.INFO, "용도 변경이 완료되었습니다.");
            } else {
                InformationMessage.InformationShowDialog("용도 변경에 실패했습니다.");
                InsertLog(LogEnum.INFO, "용도 변경에 실패했습니다.");
            }
            return state;
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }
    }
}
