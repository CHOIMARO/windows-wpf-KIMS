using Core.StaticAttribute.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.loginfo {
    public class InsertLogInfoItemUseCase : UseCase {
        public void Execute(dynamic logEnum, string message) {
            StaticAttribute.Function.logInfoService.InsertLogInfoItem(logEnum.ToString(), message);
        }
    }
}
