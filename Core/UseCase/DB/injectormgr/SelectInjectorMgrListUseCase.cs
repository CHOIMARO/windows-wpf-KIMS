using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.injectormgr {
    public class SelectInjectorMgrListUseCase {
        public dynamic Execute(int injectorIdx ) {
            return StaticAttribute.Function.injectorService.SelectInjectorMgrList(injectorIdx);
        }
    }
}
