namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("pposeinfo")]
    public partial class pposeinfo
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        [Required]
        [StringLength(50)]
        public string ppose { get; set; }

        [Required]
        [StringLength(20)]
        public string uninum { get; set; }

        [Required]
        [StringLength(50)]
        public string rank { get; set; }

        [Required]
        [StringLength(50)]
        public string uname { get; set; }

        [Required]
        [StringLength(1)]
        public string stat { get; set; }
    }
}
