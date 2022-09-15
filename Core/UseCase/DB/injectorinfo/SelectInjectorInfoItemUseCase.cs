using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.injectorinfo {
    public class SelectInjectorInfoItemUseCase : UseCase {
        public dynamic Execute(string injectorId) {
            return StaticAttribute.Function.injectorService.SelectInjectorInfoItem(injectorId);
        }
    }
}
