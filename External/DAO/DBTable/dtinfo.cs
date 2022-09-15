namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("dtinfo")]
    public partial class dtinfo
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        public int ijidx { get; set; }

        [Required]
        [StringLength(20)]
        public string mdid { get; set; }

        [Required]
        [StringLength(50)]
        public string logid { get; set; }

        [Required]
        [StringLength(20)]
        public string mdip { get; set; }

        [Required]
        [StringLength(50)]
        public string hw { get; set; }

        [Required]
        [StringLength(50)]
        public string sn { get; set; }

        [Required]
        [StringLength(50)]
        public string dpt { get; set; }

        [Required]
        [StringLength(50)]
        public string ppose { get; set; }

        [Required]
        [StringLength(10)]
        public string stat { get; set; }
    }
}
