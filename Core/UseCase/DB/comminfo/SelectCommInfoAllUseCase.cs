using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.DAO;

namespace Core.UseCase.DB.comminfo {
    public class SelectCommInfoAllUseCase : UseCase {
        public dynamic Execute() {
            return StaticAttribute.Function.commInfoService.SelectCommInfoItem();
        }
    }
}
