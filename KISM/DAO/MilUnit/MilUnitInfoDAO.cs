using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KISM.DAO.MilUnit {
    public class MilUnitInfoDAO {
        public string Num { get; set; }
        public int Idx { get; set; }
        public string Timestamp { get; set; }
        public string Grp { get; set; }
        public string UniNum { get; set; }
        public string Rank { get; set; }
        public string UserName { get; set; }
        public string Stat { get; set; }
    }
}

//MilUnitDataRow.Columns.Add("순번");
//MilUnitDataRow.Columns.Add("등록일자");
//MilUnitDataRow.Columns.Add("부대 명");
//MilUnitDataRow.Columns.Add("군번");
//MilUnitDataRow.Columns.Add("계급");
//MilUnitDataRow.Columns.Add("성명");
//MilUnitDataRow.Columns.Add("상태");