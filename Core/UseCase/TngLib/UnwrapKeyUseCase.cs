using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.TngLib {
    public class UnwrapKeyUseCase : UseCase {
        public int Execute(int keySize, ref byte[] buf, ref int bufLen, byte[] idStr, byte[] moduleIdStr, int doPadding) {
            return StaticAttribute.Function.coreEngineService.TngUnwrappedSecretKey(keySize, ref buf, ref bufLen, idStr, moduleIdStr, doPadding);
        }
    }
}
