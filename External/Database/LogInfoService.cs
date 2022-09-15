using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {
    public class LogInfoService : Service {
        public bool InsertLogInfoItem(string logType, string message) {
            using (var db = new KISMContext()) {
                db.loginfo.Add(new loginfo {
                    timestamp = DateTime.Now,
                    type = logType,
                    message = message
                });
                
                try {
                    db.SaveChanges();
                } catch (DbEntityValidationException ex) {
                    // Retrieve the error messages as a list of strings.
                    var errorMessages = ex.EntityValidationErrors
                            .SelectMany(x => x.ValidationErrors)
                            .Select(x => x.ErrorMessage);

                    // Join the list to a single string.
                    var fullErrorMessage = string.Join("; ", errorMessages);

                    // Combine the original exception message with the new one.
                    var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                    // Throw a new DbEntityValidationException with the improved exception message.
                    throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
                }
                return true;
            }
        }

        public List<loginfo> SelectLogInfoAll() {
            List<loginfo> logInfoList = new List<loginfo>();
            using (var db = new KISMContext()) {
                var infoList = from info in db.loginfo
                               select info;
                logInfoList = infoList.ToList();
            }
            return logInfoList;
        }
    }
}
