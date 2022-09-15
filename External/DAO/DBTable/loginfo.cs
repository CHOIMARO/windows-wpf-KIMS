namespace External.DAO.DBTable {
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("loginfo")]
    public partial class loginfo {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        [Required]
        [StringLength(50)]
        public string type { get; set; }

        [Required]
        [StringLength(256)]
        public string message { get; set; }
    }
}
