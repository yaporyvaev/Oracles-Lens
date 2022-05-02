using Microsoft.EntityFrameworkCore.Migrations;

namespace LeagueActivityBot.Database.Migrations
{
    public partial class RealSummonerName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "Summoners",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealName",
                table: "Summoners");
        }
    }
}
