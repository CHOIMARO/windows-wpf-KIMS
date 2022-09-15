using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.TngLib {
    public class FindAvailableRndTypeUseCase {
        public int Execute(int rndType) {
            return StaticAttribute.Function.coreEngineService.TngIsAvailableRandomType(rndType);
        }
    }
}
