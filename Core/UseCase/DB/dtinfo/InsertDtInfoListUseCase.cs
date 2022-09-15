using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.dtinfo {
    public class InsertDtInfoListUseCase : UseCase{
        public int Execute(dynamic fromKIS100LogList, dynamic dtInfoAll) {
            int state = -1;
            int count = 0;

            List<string> dtInfoLogIdList = new List<string>();

            foreach (var dtInfo in dtInfoAll) {
                dtInfoLogIdList.Add(dtInfo.logid);
            }

            foreach (var fromKIS100Log in fromKIS100LogList) {
                if (dtInfoLogIdList.Contains(CalculateLogId(fromKIS100Log))) {
                    count++;
                } else {
                    StaticAttribute.Function.distributionService.InsertDtInfoItem(fromKIS100Log);
                }
            }
            if(count == 0) { //중복 없음
                state = 1;
            }else if (count == fromKIS100LogList.Count) { //모두 중복
                state = 0;
            }else { // 부분 중복
                state = 2;
            }
            return state;
        }

        public string CalculateLogId(dynamic kis100LogData) {
            return StaticAttribute.Function.encryptionCommand.SHA1Hash(kis100LogData.timestamp + kis100LogData.name + kis100LogData.ip + kis100LogData.stat);
        }
    }
}
