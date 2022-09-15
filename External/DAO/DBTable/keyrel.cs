namespace External.DAO.DBTable
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("keyrel")]
    public partial class keyrel
    {
        [Key]
        public int idx { get; set; }

        public DateTime timestamp { get; set; }

        public int ijidx { get; set; }

        [Required]
        [StringLength(50)]
        public string dpt { get; set; }

        [Required]
        [StringLength(50)]
        public string ppose { get; set; }

        public DateTime stdate { get; set; }

        public DateTime expdate { get; set; }

        public DateTime? dtdate { get; set; }

        public DateTime? deldate { get; set; }

        [Required]
        [StringLength(2048)]
        public string wkey { get; set; }

        [Required]
        [StringLength(5)]
        public string stat { get; set; }
    }
}
