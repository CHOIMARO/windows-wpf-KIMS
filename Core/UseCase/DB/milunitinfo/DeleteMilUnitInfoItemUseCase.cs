using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.milunitinfo {
    public class DeleteMilUnitInfoItemUseCase : UseCase{
        public bool Execute(int unitIdx) {
            return StaticAttribute.Function.militaryUnitService.DeleteMilUnitInfoItem(unitIdx);
        }
    }
}
