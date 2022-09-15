namespace External.DAO.DBTable {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    [Table("serialinfo")]
    public partial class serialinfo {
        [Key]
        public int idx { get; set; }

        [Required]
        [StringLength(256)]
        public string timestamp { get; set; }

        [Required]
        [StringLength(256)]
        public string port { get; set; }

        [Required]
        [StringLength(256)]
        public string baud_rate { get; set; }

        [Required]
        [StringLength(256)]
        public string parity { get; set; }

        [Required]
        [StringLength(256)]
        public string data_bit { get; set; }

        [Required]
        [StringLength(256)]
        public string stop_bit { get; set; }
    }
}
