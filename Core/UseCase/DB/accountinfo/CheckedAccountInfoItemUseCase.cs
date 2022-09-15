using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.accountinfo {
    public class CheckedAccountInfoItemUseCase : UseCase { 
        public dynamic Execute(dynamic info) {
            return StaticAttribute.Function.accountService.CheckedAuth(info);
        }
    }
}
