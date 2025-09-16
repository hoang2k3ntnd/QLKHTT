using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OnlineCourse.Migrations
{
    /// <inheritdoc />
    public partial class LogAndEmail : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Users_UserId",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Logs",
                newName: "UserName");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Logs",
                newName: "ExtraProperties");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_UserId",
                table: "Logs",
                newName: "IX_Logs_UserName");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "Logs",
                type: "nvarchar(45)",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "ApplicationName",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BrowserInfo",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Changes",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CorrelationId",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Duration",
                table: "Logs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<string>(
                name: "Exception",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "HttpMethod",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RequestUrl",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "StatusCode",
                table: "Logs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Users_UserName",
                table: "Logs",
                column: "UserName",
                principalTable: "Users",
                principalColumn: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logs_Users_UserName",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "ApplicationName",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "BrowserInfo",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Changes",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "CorrelationId",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "Exception",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "HttpMethod",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "RequestUrl",
                table: "Logs");

            migrationBuilder.DropColumn(
                name: "StatusCode",
                table: "Logs");

            migrationBuilder.RenameColumn(
                name: "UserName",
                table: "Logs",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ExtraProperties",
                table: "Logs",
                newName: "Status");

            migrationBuilder.RenameIndex(
                name: "IX_Logs_UserName",
                table: "Logs",
                newName: "IX_Logs_UserId");

            migrationBuilder.AlterColumn<string>(
                name: "IP",
                table: "Logs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(45)",
                oldMaxLength: 45);

            migrationBuilder.AddForeignKey(
                name: "FK_Logs_Users_UserId",
                table: "Logs",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "UserId");
        }
    }
}
