using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace LeagueActivityBot.Database.Migrations
{
    public partial class WeightScore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ScoreWeights",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    IsDeleted = table.Column<bool>(nullable: false),
                    Kda = table.Column<double>(nullable: false),
                    Level = table.Column<double>(nullable: false),
                    Gold = table.Column<double>(nullable: false),
                    CcTime = table.Column<double>(nullable: false),
                    DmgToChampions = table.Column<double>(nullable: false),
                    DmgTaken = table.Column<double>(nullable: false),
                    DmgMitigated = table.Column<double>(nullable: false),
                    DmgHealed = table.Column<double>(nullable: false),
                    DmgShielded = table.Column<double>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ScoreWeights", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ScoreWeights");
        }
    }
}
