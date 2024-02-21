using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMindHub.Migrations
{
    /// <inheritdoc />
    public partial class correciondetabla : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientLoanLoan");

            migrationBuilder.CreateIndex(
                name: "IX_ClientLoans_LoanId",
                table: "ClientLoans",
                column: "LoanId");

            migrationBuilder.AddForeignKey(
                name: "FK_ClientLoans_Loans_LoanId",
                table: "ClientLoans",
                column: "LoanId",
                principalTable: "Loans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ClientLoans_Loans_LoanId",
                table: "ClientLoans");

            migrationBuilder.DropIndex(
                name: "IX_ClientLoans_LoanId",
                table: "ClientLoans");

            migrationBuilder.CreateTable(
                name: "ClientLoanLoan",
                columns: table => new
                {
                    ClientLoanId = table.Column<long>(type: "bigint", nullable: false),
                    LoanId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLoanLoan", x => new { x.ClientLoanId, x.LoanId });
                    table.ForeignKey(
                        name: "FK_ClientLoanLoan_ClientLoans_ClientLoanId",
                        column: x => x.ClientLoanId,
                        principalTable: "ClientLoans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ClientLoanLoan_Loans_LoanId",
                        column: x => x.LoanId,
                        principalTable: "Loans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ClientLoanLoan_LoanId",
                table: "ClientLoanLoan",
                column: "LoanId");
        }
    }
}
