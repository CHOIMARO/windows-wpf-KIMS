using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.injectorinfo {
    public class InsertInjectorInfoItemUseCase : UseCase {
        public void Execute(dynamic kIS100Hello) {
            StaticAttribute.Function.injectorService.InsertInfo(new External.DAO.JSON.KIS100.FromKIS100Hello {
                id = kIS100Hello.id,
                fw = kIS100Hello.fw,
                hw = kIS100Hello.hw,
                sn = kIS100Hello.sn
            });
        }
    }
}
