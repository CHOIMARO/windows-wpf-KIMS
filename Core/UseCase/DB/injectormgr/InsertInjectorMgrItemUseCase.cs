using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.injectormgr {
    public class InsertInjectorMgrItemUseCase : UseCase{
        public void Execute(string id, string pw) {
            StaticAttribute.Function.injectorService.InsertInjectorMgrInfo(id);
        }
    }
}
