using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.pposeinfo {
    public class DeletePposeInfoItemUseCase : UseCase{
        public bool Execute(int pposeIdx) {
            return StaticAttribute.Function.purposeInfoService.DeletePposeInfoItem(pposeIdx);
        }
    }
}
