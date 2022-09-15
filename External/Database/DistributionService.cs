using External.DAO;
using External.DAO.DBTable;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace External.Database {
    public class DistributionService : Service {
        public bool InsertDtInfoItem(dynamic fromKIS100Log) {
            bool state = false;

            try {
                using (var db = new KISMContext()) {
                    var dt = db.Set<dtinfo>();
                    dt.Add(new dtinfo {
                        timestamp = fromKIS100Log.timestamp,
                        ijidx = StaticAttribute.Function.ijidx,
                        mdid = fromKIS100Log.name,
                        logid = StaticAttribute.Function.encryptionCommand.SHA1Hash(
                        fromKIS100Log.timestamp + fromKIS100Log.name + fromKIS100Log.ip + fromKIS100Log.stat),
                        mdip = fromKIS100Log.ip.Length > 0 ? fromKIS100Log.ip : "-",
                        hw = fromKIS100Log.hw,
                        sn = fromKIS100Log.sn,
                        dpt = fromKIS100Log.grp,
                        ppose = fromKIS100Log.ppo,
                        stat = fromKIS100Log.stat.ToString()
                    });
                    db.SaveChanges();
                }

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
            } catch (Exception ex) {
                StaticAttribute.Function.logCommand.errorLog("[External.DistributionServiceImpl.insertDtInfo] " + ex.Message);
                StaticAttribute.Function.logInfoService.InsertLogInfoItem("ERROR", "배포 이력을 등록하는 데 실패했습니다.");
            }
            return state;
        }

        public List<dtinfo> SelectDtInfoAll() {
            List<dtinfo> dtinfoList = new List<dtinfo>();
            using (var db = new KISMContext()) {
                var dtInfo = from info in db.dtinfo
                             //where info.ijidx == StaticAttribute.Function.ijidx
                             select info;
                dtinfoList = dtInfo.ToList();
            }
            return dtinfoList;
        }
    }
}
