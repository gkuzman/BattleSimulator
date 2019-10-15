using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleSimulator.DAL.Migrations.TrackingContext
{
    public partial class UpdateBattleEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "JobId",
                table: "Battles",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "JobId",
                table: "Battles");
        }
    }
}
