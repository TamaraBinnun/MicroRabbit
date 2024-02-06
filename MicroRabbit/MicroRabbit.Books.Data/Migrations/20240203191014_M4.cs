using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroRabbit.Books.Data.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "OrderedBooks",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedBooks_OrderId_BookId",
                table: "OrderedBooks",
                columns: new[] { "OrderId", "BookId" },
                unique: true,
                filter: "[BookId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderedBooks_OrderId_ExternalBookId",
                table: "OrderedBooks",
                columns: new[] { "OrderId", "ExternalBookId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_OrderedBooks_OrderId_BookId",
                table: "OrderedBooks");

            migrationBuilder.DropIndex(
                name: "IX_OrderedBooks_OrderId_ExternalBookId",
                table: "OrderedBooks");

            migrationBuilder.AlterColumn<string>(
                name: "ISBN",
                table: "OrderedBooks",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(20)",
                oldMaxLength: 20);
        }
    }
}
