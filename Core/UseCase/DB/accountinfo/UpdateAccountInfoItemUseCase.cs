using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.accountinfo {
    public class UpdateAccountInfoItemUseCase : UseCase {
        public bool Execute(dynamic accountInfoItem) {
            return StaticAttribute.Function.accountService.UpdateAccountInfoItem(accountInfoItem);
        }
    }
}
