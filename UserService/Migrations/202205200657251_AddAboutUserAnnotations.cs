namespace Identity.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddAboutUserAnnotations : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.AboutUsers", "Name", c => c.String(nullable: false, maxLength: 60));
            AlterColumn("dbo.AboutUsers", "Description", c => c.String(nullable: false, maxLength: 1000));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.AboutUsers", "Description", c => c.String());
            AlterColumn("dbo.AboutUsers", "Name", c => c.String());
        }
    }
}
