using Microsoft.EntityFrameworkCore.Migrations;

namespace BattleSimulator.DAL.Migrations.TrackingContext
{
    public partial class HangfireDB : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("IF NOT EXISTS(SELECT * FROM sys.databases WHERE NAME = 'Hangfire') " +
                                 "CREATE DATABASE Hangfire", true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // do not drop the database since we are unsure did it exist before for other apps
        }
    }
}
