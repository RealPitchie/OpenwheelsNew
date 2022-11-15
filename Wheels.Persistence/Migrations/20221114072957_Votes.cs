using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Wheels.Persistence.Migrations
{
    public partial class Votes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        { 
            migrationBuilder.CreateTable(
                name: "Vote",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    VoteType = table.Column<string>(type: "text", nullable: false),
                    User = table.Column<string>(type: "text", nullable: true),
                    CommentId = table.Column<string>(type: "text", nullable: true),
                    PostId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vote", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vote_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Vote_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Vote_CommentId",
                table: "Vote",
                column: "CommentId");

            migrationBuilder.CreateIndex(
                name: "IX_Vote_PostId",
                table: "Vote",
                column: "PostId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Vote");

            migrationBuilder.DropColumn(
                name: "WasDeleted",
                table: "Posts");
        }
    }
}
