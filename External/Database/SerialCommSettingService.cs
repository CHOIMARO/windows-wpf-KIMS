using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {
    public class SerialCommSettingService : Service {
        public serialinfo getSerialCommData() {
            serialinfo defaultData = new serialinfo();
            using (var db = new KISMContext()) {

                var data = from info in db.serialinfo
                           select info;

                if (data.SingleOrDefault() != null) {
                    return data.SingleOrDefault();
                } else {
                    defaultData = new serialinfo {
                        timestamp = DateTime.Now.ToString(),
                        port = "COM1",
                        baud_rate = "115200",
                        parity = "None",
                        data_bit = "8",
                        stop_bit = "1",
                    };
                    var serialInfo = db.Set<serialinfo>();
                    serialInfo.Add(defaultData);
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
                    return defaultData;
                }

            }
        }

        public bool update(dynamic settingInfo) {
            using (var db = new KISMContext()) {

                var data = from info in db.serialinfo
                           select info;
                serialinfo selectData = data.ToList()[0];
                selectData.timestamp = DateTime.Now.ToString("yyyy-MM-dd");
                selectData.port = settingInfo.port;
                selectData.baud_rate = settingInfo.baudRate;
                selectData.parity = settingInfo.parity;
                selectData.data_bit = settingInfo.dataBit;
                selectData.stop_bit = settingInfo.stopBit;

                try {
                    db.SaveChanges();
                    return true;
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
            }
        }
    }
}
