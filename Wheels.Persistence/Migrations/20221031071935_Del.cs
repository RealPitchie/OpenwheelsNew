using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheels.Persistence.Migrations
{
    public partial class Del : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                            name: "WasDeleted",
                            table: "Posts",
                            type: "boolean",
                            nullable: false,
                            defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropColumn(
                                                     name: "WasDeleted",
                                                     table: "Posts");
        }
    }
}
