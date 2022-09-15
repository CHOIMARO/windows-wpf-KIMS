namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("injectorinfo")]
    public partial class injectorinfo
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        [Required]
        [StringLength(250)]
        public string ijid { get; set; }

        [Required]
        [StringLength(250)]
        public string hw { get; set; }

        [Required]
        [StringLength(100)]
        public string fw { get; set; }

        [Required]
        [StringLength(100)]
        public string sn { get; set; }

        [Required]
        [StringLength(1)]
        public string stat { get; set; }
    }
}
