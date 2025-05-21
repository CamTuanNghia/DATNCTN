namespace WebSiteMCSport.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class dss : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.AspNetUsers", "Avatar", c => c.String());
            AddColumn("dbo.AspNetUsers", "Address", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.AspNetUsers", "Address");
            DropColumn("dbo.AspNetUsers", "Avatar");
        }
    }
}
