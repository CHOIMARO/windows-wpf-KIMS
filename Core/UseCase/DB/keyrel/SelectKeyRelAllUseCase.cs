using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.keyrel {
    public class SelectKeyRelAllUseCase : UseCase {
        public dynamic Execute() {
            return StaticAttribute.Function.keyRelationService.SelectKeyRelAll();
        }
    }
}
