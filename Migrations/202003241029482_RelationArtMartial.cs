namespace Dojo.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RelationArtMartial : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.SamouraiArtMartials",
                c => new
                    {
                        Samourai_Id = c.Int(nullable: false),
                        ArtMartial_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Samourai_Id, t.ArtMartial_Id })
                .ForeignKey("dbo.Samourais", t => t.Samourai_Id, cascadeDelete: true)
                .ForeignKey("dbo.ArtMartials", t => t.ArtMartial_Id, cascadeDelete: true)
                .Index(t => t.Samourai_Id)
                .Index(t => t.ArtMartial_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.SamouraiArtMartials", "ArtMartial_Id", "dbo.ArtMartials");
            DropForeignKey("dbo.SamouraiArtMartials", "Samourai_Id", "dbo.Samourais");
            DropIndex("dbo.SamouraiArtMartials", new[] { "ArtMartial_Id" });
            DropIndex("dbo.SamouraiArtMartials", new[] { "Samourai_Id" });
            DropTable("dbo.SamouraiArtMartials");
        }
    }
}
