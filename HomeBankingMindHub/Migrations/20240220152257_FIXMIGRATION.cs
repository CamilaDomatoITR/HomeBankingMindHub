using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMindHub.Migrations
{
    /// <inheritdoc />
    public partial class FIXMIGRATION : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreationDate",
                table: "Transaction",
                newName: "Date");

            migrationBuilder.CreateIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction",
                column: "AccountId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction",
                column: "AccountId",
                principalTable: "Account",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Account_AccountId",
                table: "Transaction");

            migrationBuilder.DropIndex(
                name: "IX_Transaction_AccountId",
                table: "Transaction");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Transaction",
                newName: "CreationDate");
        }
    }
}
