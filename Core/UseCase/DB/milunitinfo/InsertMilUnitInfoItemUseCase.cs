using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.milunitinfo {
    public class InsertMilUnitInfoItemUseCase : UseCase {
        public bool Execute(string grp) {
            return StaticAttribute.Function.militaryUnitService.InsertMilUnitInfoItem(grp);
        }
    }
}
