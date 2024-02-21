using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HomeBankingMindHub.Migrations
{
    /// <inheritdoc />
    public partial class addLoanEntiity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClientLoans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Payments = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ClientId = table.Column<long>(type: "bigint", nullable: false),
                    LoanId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClientLoans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ClientLoans_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Loans",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MaxAmount = table.Column<double>(type: "float", nullable: false),
                    Payments = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Loans", x => x.Id);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_ClientLoans_ClientId",
                table: "ClientLoans",
                column: "ClientId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClientLoanLoan");

            migrationBuilder.DropTable(
                name: "ClientLoans");

            migrationBuilder.DropTable(
                name: "Loans");
        }
    }
}
