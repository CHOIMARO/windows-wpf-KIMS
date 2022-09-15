using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.UseCase.DB.keyrel {
    public class InsertKeyRelItemUseCase : UseCase {
        public bool Execute(string wrappedKey, string dpt, string ppose, string usingDate, string expDate) {
            int expirationDate = getExpMonths(expDate);
            return StaticAttribute.Function.keyRelationService.InsertKeyRelItem(wrappedKey, dpt, ppose, usingDate, expirationDate);
        }

        public int getExpMonths(string expDate) {
            int expDateConvert = -1;
            if (expDate.Length == 3) {
                if (expDate.Equals("무제한")) {
                    expDateConvert = 12000;
                } else {
                    expDateConvert = Convert.ToInt32(expDate.Substring(0, 1));
                }

            } else if (expDate.Length == 4) {
                expDateConvert = Convert.ToInt32(expDate.Substring(0, 2));
            }
            return expDateConvert;
        }
    }
}
