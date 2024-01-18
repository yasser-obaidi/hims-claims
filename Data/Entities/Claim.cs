// Ignore Spelling: TPA

using ClaimManagement.Enums;
using ClamManagement.Data.Entities.Commen;

namespace ClaimManagement.Data.Entities
{
    public class Claim : BaseEntity
    {
        public int Id { get; set; }
        public string? ClaimNumber  { get; set; }
        public DateTime ClaimDate { get; set; }
        public int TPAId { get; set; }
        public TPA? TPA { get; set; }
        public string? TPAClaimReferenceNumber { get; set; }
        public int NetworkProviderId { get; set; }
        public NetworkProvider? NetworkProvider { get; set; }
        public string? NetworkProviderInvoiceNumber { get; set; }
        public DateTime ProcedureDate { get; set; }
        public int ServiceCategoryId { get; set; }
        public string? ServiceName { get; set; }
        public string? CardNo { get; set;}
        public string? PatientName {  get; set; }
        public string? DiagnosticCode { get; set; } 
        public string? DiagnosticDescription { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? TreatmentCountry {  get; set; }
        public bool IsReimbursement { get; set; }
        public decimal AmountClaimedOriginalCurrency { get; set; }
        public string? OriginalCurrencyCode { get; set; }
        public decimal AmountClaimedPlanCurrency { get; set; }
        public string? PlanCurrencyCode { get; set; }
        public decimal AmountApprovedOriginalCurrency { get; set; }
        public decimal AmountApprovedPlanCurrency { get; set; }
        public string? CoPaymentOriginalCurrency { get; set; }
        public string? CoPaymentPlanCurrency { get; set; }
        public decimal? AmountPaidOriginalCurrency { get; set; }
        public decimal? AmountPaidPlanCurrency { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public int PolicyId {  get; set; }
        public ClaimStatus Status { get; set; }
        public Result AdjudicationResult { get; set; }

    }
}
