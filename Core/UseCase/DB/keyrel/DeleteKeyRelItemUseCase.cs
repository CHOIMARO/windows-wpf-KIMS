using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.keyrel {
    public class DeleteKeyRelItemUseCase : UseCase {
        public bool Execute(int idx) {
            return StaticAttribute.Function.keyRelationService.DeleteKeyRelItem(idx);
        }
    }
}
