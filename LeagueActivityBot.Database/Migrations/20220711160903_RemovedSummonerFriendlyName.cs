using Microsoft.EntityFrameworkCore.Migrations;

namespace LeagueActivityBot.Database.Migrations
{
    public partial class RemovedSummonerFriendlyName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RealName",
                table: "Summoners");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RealName",
                table: "Summoners",
                type: "text",
                nullable: true);
        }
    }
}
