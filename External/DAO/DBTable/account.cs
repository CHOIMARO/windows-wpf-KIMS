namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("account")]
    public partial class account
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        [Required]
        [StringLength(50)]
        public string uid { get; set; }

        [StringLength(128)]
        public string upw { get; set; }

        [StringLength(50)]
        public string uname { get; set; }

        [StringLength(50)]
        public string email { get; set; }

        [StringLength(20)]
        public string tel { get; set; }

        [Required]
        [StringLength(20)]
        public string uninum { get; set; }

        [StringLength(50)]
        public string dpt { get; set; }

        [StringLength(50)]
        public string rank { get; set; }

        [StringLength(10)]
        public string grp { get; set; }

        public int pmit { get; set; }

        [Required]
        [StringLength(1)]
        public string stat { get; set; }

        [StringLength(1024)]
        public string note { get; set; }
    }
}
