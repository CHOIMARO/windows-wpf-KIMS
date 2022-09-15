using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.dtinfo {
    public class SelectDtInfoAllUseCase : UseCase {
        public dynamic Execute() {
            return StaticAttribute.Function.distributionService.SelectDtInfoAll();
        }
    }
}
