namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("comminfo")]
    public partial class comminfo
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        [Required]
        [StringLength(50)]
        public string ip { get; set; }

        public int port { get; set; }
    }
}
