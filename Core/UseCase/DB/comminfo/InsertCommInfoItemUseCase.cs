using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.comminfo {
    public class InsertCommInfoItemUseCase : UseCase {
        public bool Execute(string ip, int port) {
            return StaticAttribute.Function.commInfoService.InsertCommInfoItem(ip, port);
        }
    }
}
