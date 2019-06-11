using Microsoft.EntityFrameworkCore.Migrations;

namespace trojkaty_api.DataAccess.Migrations
{
    public partial class addroletouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "ValidateQuestions",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "Role",
                table: "Users",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Role",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "QuestionId",
                table: "ValidateQuestions",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
