using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace External.DAO.DBTable {
    public partial class KISMContext : DbContext {
        public KISMContext()
            : base("name=KISMContext") {
        }

        public virtual DbSet<account> account { get; set; }
        public virtual DbSet<comminfo> comminfo { get; set; }
        public virtual DbSet<dtinfo> dtinfo { get; set; }
        public virtual DbSet<injectorinfo> injectorinfo { get; set; }
        public virtual DbSet<injectormgr> injectormgr { get; set; }
        public virtual DbSet<keyrel> keyrel { get; set; }
        public virtual DbSet<milunitinfo> milunitinfo { get; set; }
        public virtual DbSet<pposeinfo> pposeinfo { get; set; }
        public virtual DbSet<loginfo> loginfo { get; set; }
        public virtual DbSet<serialinfo> serialinfo{ get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder.Entity<account>()
                .Property(e => e.uid)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.upw)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.uname)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.email)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.tel)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.uninum)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.dpt)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.rank)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.grp)
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.stat)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<account>()
                .Property(e => e.note)
                .IsUnicode(false);

            modelBuilder.Entity<comminfo>()
                .Property(e => e.ip)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.mdid)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.logid)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.mdip)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.hw)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.sn)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.dpt)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.ppose)
                .IsUnicode(false);

            modelBuilder.Entity<dtinfo>()
                .Property(e => e.stat)
                .IsUnicode(false);

            modelBuilder.Entity<injectorinfo>()
                .Property(e => e.ijid)
                .IsUnicode(false);

            modelBuilder.Entity<injectorinfo>()
                .Property(e => e.hw)
                .IsUnicode(false);

            modelBuilder.Entity<injectorinfo>()
                .Property(e => e.fw)
                .IsUnicode(false);

            modelBuilder.Entity<injectorinfo>()
                .Property(e => e.sn)
                .IsUnicode(false);

            modelBuilder.Entity<injectorinfo>()
                .Property(e => e.stat)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<injectormgr>()
                .Property(e => e.uid)
                .IsUnicode(false);

            modelBuilder.Entity<keyrel>()
                .Property(e => e.dpt)
                .IsUnicode(false);

            modelBuilder.Entity<keyrel>()
                .Property(e => e.ppose)
                .IsUnicode(false);

            modelBuilder.Entity<keyrel>()
                .Property(e => e.wkey)
                .IsUnicode(false);

            modelBuilder.Entity<keyrel>()
                .Property(e => e.stat)
                .IsUnicode(false);

            modelBuilder.Entity<milunitinfo>()
                .Property(e => e.unit)
                .IsUnicode(false);

            modelBuilder.Entity<milunitinfo>()
                .Property(e => e.uninum)
                .IsUnicode(false);

            modelBuilder.Entity<milunitinfo>()
                .Property(e => e.rank)
                .IsUnicode(false);

            modelBuilder.Entity<milunitinfo>()
                .Property(e => e.uname)
                .IsUnicode(false);

            modelBuilder.Entity<milunitinfo>()
                .Property(e => e.stat)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<pposeinfo>()
                .Property(e => e.ppose)
                .IsUnicode(false);

            modelBuilder.Entity<pposeinfo>()
                .Property(e => e.uninum)
                .IsUnicode(false);

            modelBuilder.Entity<pposeinfo>()
                .Property(e => e.rank)
                .IsUnicode(false);

            modelBuilder.Entity<pposeinfo>()
                .Property(e => e.uname)
                .IsUnicode(false);

            modelBuilder.Entity<pposeinfo>()
                .Property(e => e.stat)
                .IsFixedLength()
                .IsUnicode(false);
            modelBuilder.Entity<serialinfo>()
                .Property(e => e.timestamp)
                .IsUnicode(false);

            modelBuilder.Entity<serialinfo>()
                .Property(e => e.port)
                .IsUnicode(false);

            modelBuilder.Entity<serialinfo>()
                .Property(e => e.baud_rate)
                .IsUnicode(false);

            modelBuilder.Entity<serialinfo>()
                .Property(e => e.parity)
                .IsUnicode(false);

            modelBuilder.Entity<serialinfo>()
                .Property(e => e.data_bit)
                .IsUnicode(false);

            modelBuilder.Entity<serialinfo>()
                .Property(e => e.stop_bit)
                .IsUnicode(false);
            modelBuilder.Entity<loginfo>()
                .Property(e => e.type)
                .IsUnicode(false);
            modelBuilder.Entity<loginfo>()
                .Property(e => e.message)
                .IsUnicode(false);
        }
    }
}
