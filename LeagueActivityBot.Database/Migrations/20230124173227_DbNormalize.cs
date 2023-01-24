using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LeagueActivityBot.Database.Migrations
{
    public partial class DbNormalize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LeaguePoints",
                table: "Summoners");

            migrationBuilder.DropColumn(
                name: "Rank",
                table: "Summoners");

            migrationBuilder.DropColumn(
                name: "Tier",
                table: "Summoners");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "Summoners",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GameParticipants",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "GameInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LeagueInfos",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    SummonerId = table.Column<int>(nullable: false),
                    LeagueType = table.Column<int>(nullable: false),
                    Tier = table.Column<int>(nullable: false),
                    Rank = table.Column<int>(nullable: false),
                    LeaguePoints = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LeagueInfos", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LeagueInfos_Summoners_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LeagueInfos_SummonerId",
                table: "LeagueInfos",
                column: "SummonerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LeagueInfos");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "Summoners");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GameParticipants");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "GameInfos");

            migrationBuilder.AddColumn<int>(
                name: "LeaguePoints",
                table: "Summoners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rank",
                table: "Summoners",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Tier",
                table: "Summoners",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
