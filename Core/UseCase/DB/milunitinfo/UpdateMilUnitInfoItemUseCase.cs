using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.milunitinfo {
    public class UpdateMilUnitInfoItemUseCase : UseCase{
        public bool Execute(int idx, string unit) {
            return StaticAttribute.Function.militaryUnitService.UpdateMilUnitInfoItem(idx, unit);
        }
    }
}
