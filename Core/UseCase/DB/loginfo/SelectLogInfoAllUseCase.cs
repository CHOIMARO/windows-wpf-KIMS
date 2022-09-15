using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.loginfo {
    public class SelectLogInfoAllUseCase : UseCase{
        public dynamic Execute() {
            return StaticAttribute.Function.logInfoService.SelectLogInfoAll();
        }
    }
}
