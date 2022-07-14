using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LeagueActivityBot.Database.Migrations
{
    public partial class GameParticipantStat : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SummonerNamesJson",
                table: "GameInfos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameStartTime",
                table: "GameInfos",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "timestamp without time zone");

            migrationBuilder.AddColumn<long>(
                name: "GameDurationInSeconds",
                table: "GameInfos",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "GameEnded",
                table: "GameInfos",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "GameParticipants",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    SummonerId = table.Column<int>(nullable: false),
                    GameInfoId = table.Column<int>(nullable: false),
                    Win = table.Column<bool>(nullable: true),
                    Kills = table.Column<int>(nullable: true),
                    Deaths = table.Column<int>(nullable: true),
                    Assists = table.Column<int>(nullable: true),
                    CreepScore = table.Column<int>(nullable: true),
                    VisionScore = table.Column<int>(nullable: true),
                    ChampionId = table.Column<int>(nullable: true),
                    ChampionName = table.Column<string>(nullable: true),
                    DetectorWardsPlaced = table.Column<int>(nullable: true),
                    FirstBloodKill = table.Column<bool>(nullable: true),
                    PentaKills = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GameParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GameParticipants_GameInfos_GameInfoId",
                        column: x => x.GameInfoId,
                        principalTable: "GameInfos",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GameParticipants_Summoners_SummonerId",
                        column: x => x.SummonerId,
                        principalTable: "Summoners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GameParticipants_GameInfoId",
                table: "GameParticipants",
                column: "GameInfoId");

            migrationBuilder.CreateIndex(
                name: "IX_GameParticipants_SummonerId",
                table: "GameParticipants",
                column: "SummonerId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GameParticipants");

            migrationBuilder.DropColumn(
                name: "GameDurationInSeconds",
                table: "GameInfos");

            migrationBuilder.DropColumn(
                name: "GameEnded",
                table: "GameInfos");

            migrationBuilder.AlterColumn<DateTime>(
                name: "GameStartTime",
                table: "GameInfos",
                type: "timestamp without time zone",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SummonerNamesJson",
                table: "GameInfos",
                type: "text",
                nullable: true);
        }
    }
}
