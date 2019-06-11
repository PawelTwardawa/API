using Microsoft.EntityFrameworkCore.Migrations;

namespace trojkaty_api.DataAccess.Migrations
{
    public partial class change_ValidateQuestion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Published",
                table: "ValidateQuestions",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Published",
                table: "ValidateQuestions");
        }
    }
}
