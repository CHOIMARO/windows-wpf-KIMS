using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.pposeinfo {
    public class InsertPposeInfoItemUseCase : UseCase{
        public bool Execute(string ppose) {
            return StaticAttribute.Function.purposeInfoService.InsertPposeInfoItem(ppose);
        }
    }
}
