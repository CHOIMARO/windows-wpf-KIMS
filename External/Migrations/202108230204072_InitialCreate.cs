namespace External.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.account",
                c => new
                    {
                        idx = c.Int(nullable: false, identity: true),
                        timestamp = c.DateTime(nullable: false),
                        uid = c.String(nullable: false, maxLength: 50, unicode: false),
                        upw = c.String(maxLength: 128, unicode: false),
                        uname = c.String(maxLength: 50, unicode: false),
                        email = c.String(maxLength: 50, unicode: false),
                        tel = c.String(maxLength: 20, unicode: false),
                        dpt = c.String(maxLength: 50, unicode: false),
                        grp = c.String(maxLength: 10, unicode: false),
                        pmit = c.Int(nullable: false),
                        stat = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                        note = c.String(maxLength: 1024, unicode: false),
                    })
                .PrimaryKey(t => t.idx);
            
            CreateTable(
                "dbo.dtinfo",
                c => new
                    {
                        idx = c.Int(nullable: false, identity: true),
                        timestamp = c.DateTime(nullable: false),
                        ijidx = c.Int(nullable: false),
                        mdid = c.String(nullable: false, maxLength: 20, unicode: false),
                        logid = c.String(nullable: false, maxLength: 50, unicode: false),
                        mdip = c.String(nullable: false, maxLength: 20, unicode: false),
                        stat = c.String(nullable: false, maxLength: 10, unicode: false),
                    })
                .PrimaryKey(t => t.idx);
            
            CreateTable(
                "dbo.injectorinfo",
                c => new
                    {
                        idx = c.Int(nullable: false, identity: true),
                        timestamp = c.DateTime(nullable: false),
                        ijid = c.String(nullable: false, maxLength: 50, unicode: false),
                        hw = c.String(maxLength: 50, unicode: false),
                        fw = c.String(maxLength: 50, unicode: false),
                        sn = c.String(maxLength: 50, unicode: false),
                        stat = c.String(nullable: false, maxLength: 1, fixedLength: true, unicode: false),
                    })
                .PrimaryKey(t => t.idx);
            
            CreateTable(
                "dbo.injectormgr",
                c => new
                    {
                        idx = c.Int(nullable: false, identity: true),
                        timestamp = c.DateTime(nullable: false),
                        ijidx = c.Int(nullable: false),
                        uid = c.String(nullable: false, maxLength: 8, unicode: false),
                    })
                .PrimaryKey(t => t.idx);
            
            CreateTable(
                "dbo.keyrel",
                c => new
                    {
                        idx = c.Int(nullable: false, identity: true),
                        timestamp = c.DateTime(nullable: false),
                        ijidx = c.Int(nullable: false),
                        dpt = c.String(nullable: false, maxLength: 50, unicode: false),
                        ppose = c.String(nullable: false, maxLength: 50, unicode: false),
                        regdate = c.DateTime(nullable: false),
                        dtdate = c.DateTime(),
                        deldate = c.DateTime(),
                        wkey = c.String(nullable: false, maxLength: 2048, unicode: false),
                        stat = c.String(nullable: false, maxLength: 5, unicode: false),
                    })
                .PrimaryKey(t => t.idx);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.keyrel");
            DropTable("dbo.injectormgr");
            DropTable("dbo.injectorinfo");
            DropTable("dbo.dtinfo");
            DropTable("dbo.account");
        }
    }
}
