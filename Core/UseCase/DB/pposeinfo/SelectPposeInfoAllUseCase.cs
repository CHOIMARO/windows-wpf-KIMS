using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.pposeinfo {
    public class SelectPposeInfoAllUseCase : UseCase {
        public dynamic Execute() {
            return StaticAttribute.Function.purposeInfoService.SelectPposeInfoAll();
        }
    }
}
