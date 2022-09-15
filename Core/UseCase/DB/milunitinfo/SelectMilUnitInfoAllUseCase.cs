using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.milunitinfo {
    public class SelectMilUnitInfoAllUseCase : UseCase {
        public dynamic Execute() {
            return StaticAttribute.Function.militaryUnitService.SelectMilUnitInfoAll();
        }
    }
}
