using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.TngLib {
    public class GenerateKeyUseCase : UseCase{
        public int Execute(ref byte[] buf, int size, int rndType) {
            return StaticAttribute.Function.coreEngineService.TngGenerateRandomNumber(ref buf, size, rndType);
        }
    }
}
