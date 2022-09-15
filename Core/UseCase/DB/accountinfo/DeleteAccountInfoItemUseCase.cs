using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.accountinfo {
    public class DeleteAccountInfoItemUseCase : UseCase {
        public bool Execute(int idx) {
            return StaticAttribute.Function.accountService.DeleteAccountInfoItem(idx);
        }
    }
}
