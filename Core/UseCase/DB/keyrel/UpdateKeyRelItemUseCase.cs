using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.keyrel {
    public class UpdateKeyRelItemUseCase : UseCase{
        public bool Execute(int idx, string injectorId) {
            return StaticAttribute.Function.keyRelationService.UpdateKeyRelItem(idx, injectorId);
        }
    }
}
