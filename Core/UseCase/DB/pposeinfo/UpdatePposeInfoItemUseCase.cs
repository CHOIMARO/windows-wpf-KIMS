using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.pposeinfo {
    public class UpdatePposeInfoItemUseCase : UseCase {
        public bool Execute(int idx, string ppose) {
            return StaticAttribute.Function.purposeInfoService.UpdatePposeInfoItem(idx, ppose);
        }
    }
}
