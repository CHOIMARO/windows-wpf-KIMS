using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.accountinfo {
    public class SelectAccountInfoAllUseCase : UseCase {
        public dynamic Execute() {
            return StaticAttribute.Function.accountService.SelectAccountInfoAll();
        }
    }
}
