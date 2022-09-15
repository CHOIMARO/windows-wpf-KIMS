namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("injectormgr")]
    public partial class injectormgr
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        public int ijidx { get; set; }

        [Required]
        [StringLength(12)]
        public string uid { get; set; }
    }
}
