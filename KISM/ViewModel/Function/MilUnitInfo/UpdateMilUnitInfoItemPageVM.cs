using KISM.StaticAttribute.Enum;
using KISM.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace KISM.ViewModel.Function.MilUnitInfo {
    public class UpdateMilUnitInfoItemPageVM : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        void onPropertyChanged([CallerMemberName] string propertyName = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        internal bool DuplicateCheckMilUnitInfoItem(int idx, string updateUnit) {
            var milUnitInfoAll = StaticAttribute.Function.selectMilUnitInfoAllUseCase.Execute();
            foreach (var milUnitInfoItem in milUnitInfoAll) {
                if (milUnitInfoItem.unit.Equals(updateUnit) && milUnitInfoItem.stat.Equals("A")) {
                    InformationMessage.InformationShowDialog("중복된 부대가 존재합니다.");
                    InsertLog(LogEnum.INFO, "중복된 부대가 존재합니다.");
                    return false;
                }
            }

            return UpdateMilUnitInfoItem(idx, updateUnit);
        }
        internal bool UpdateMilUnitInfoItem(int idx, string updateUnit) {

            bool state = StaticAttribute.Function.updateMilUnitInfoItemUseCase.Execute(idx, updateUnit);
            if (state) {
                InformationMessage.InformationShowDialog("부대 변경이 완료되었습니다.");
                InsertLog(LogEnum.INFO, "부대 변경이 완료되었습니다.");
            } else {
                InformationMessage.InformationShowDialog("부대 변경에 실패했습니다.");
                InsertLog(LogEnum.INFO, "부대 변경에 실패했습니다.");
            }
            return state;
        }
        public void InsertLog(LogEnum logEnum, string message) {
            StaticAttribute.Function.insertLogInfoItemUseCase.Execute(logEnum, message);
        }
    }
}
