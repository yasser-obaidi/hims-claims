using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace ClaimManagement.Migrations
{
    /// <inheritdoc />
    public partial class addedClaim : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Claim",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    ClaimNumber = table.Column<string>(type: "longtext", nullable: true),
                    ClaimDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TPAId = table.Column<int>(type: "int", nullable: false),
                    TPAClaimReferenceNumber = table.Column<string>(type: "longtext", nullable: true),
                    NetworkProviderId = table.Column<int>(type: "int", nullable: false),
                    NetworkProviderInvoiceNumber = table.Column<string>(type: "longtext", nullable: true),
                    ProcedureDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    ServiceCategoryId = table.Column<int>(type: "int", nullable: false),
                    ServiceName = table.Column<string>(type: "longtext", nullable: true),
                    CardNo = table.Column<string>(type: "longtext", nullable: true),
                    PatientName = table.Column<string>(type: "longtext", nullable: true),
                    DiagnosticCode = table.Column<string>(type: "longtext", nullable: true),
                    DiagnosticDescription = table.Column<string>(type: "longtext", nullable: true),
                    AdmissionDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DischargeDate = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    TreatmentCountry = table.Column<string>(type: "longtext", nullable: true),
                    IsReimbursement = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AmountClaimedOriginalCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OriginalCurrencyCode = table.Column<string>(type: "longtext", nullable: true),
                    AmountClaimedPlanCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PlanCurrencyCode = table.Column<string>(type: "longtext", nullable: true),
                    AmountApprovedOriginalCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AmountApprovedPlanCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CoPaymentOriginalCurrency = table.Column<string>(type: "longtext", nullable: true),
                    CoPaymentPlanCurrency = table.Column<string>(type: "longtext", nullable: true),
                    AmountPaidOriginalCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AmountPaidPlanCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PaymentMethod = table.Column<int>(type: "int", nullable: false),
                    Notes = table.Column<string>(type: "longtext", nullable: true),
                    PolicyId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    AdjudicationResult = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    DeletedAt = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    CreatedBy = table.Column<int>(type: "int", nullable: false),
                    UpdatedBy = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Claim", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Claim_NetworkProviders_NetworkProviderId",
                        column: x => x.NetworkProviderId,
                        principalTable: "NetworkProviders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Claim_TPAs_TPAId",
                        column: x => x.TPAId,
                        principalTable: "TPAs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_Claim_NetworkProviderId",
                table: "Claim",
                column: "NetworkProviderId");

            migrationBuilder.CreateIndex(
                name: "IX_Claim_TPAId",
                table: "Claim",
                column: "TPAId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Claim");
        }
    }
}