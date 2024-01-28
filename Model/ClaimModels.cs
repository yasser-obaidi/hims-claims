// Ignore Spelling: Ecxal

using ClaimManagement.Data.Entities;
using ClaimManagement.Enums;

namespace ClaimManagement.Model
{
    public class ClaimOutputModelSimple
    {
        public int Id { get; set; }
        public string? ClaimNumber { get; set; }
        public DateTime ClaimDate { get; set; }
        public int TPAId { get; set; }
        public string? TPAClaimReferenceNumber { get; set; }
        public int NetworkProviderId { get; set; }
        public string? NetworkProviderInvoiceNumber { get; set; }
        public DateTime ProcedureDate { get; set; }
        public int ServiceCategoryId { get; set; }
        public string? ServiceName { get; set; }
        public string? CardNo { get; set; }
        public string? PatientName { get; set; }
        public string? DiagnosticCode { get; set; }
        public string? DiagnosticDescription { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? TreatmentCountry { get; set; }
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
        public int PolicyId { get; set; }
        public ClaimStatus Status { get; set; }
        public Result AdjudicationResult { get; set; }
    }
    public class ClaimOutputModelDetailed : ClaimOutputModelSimple
    {
        public PolicyOutputModelSimple? Policy { get; set; }
        public NetworkProviderOutputModelSimple? NetworkProvider { get; set; }
        public TPAOutputModelSimple? TPA { get; set; }
    }
    public class PolicyOutputModelSimple
    {
        public string? PolicyHolder { get; set;}
        public string? PolicyNumber { get; set; }
    }
    public class ClaimEcxalSheetMappingModel
    {
        public string? ClaimNumber { get; set; }
        public string? ClaimDate { get; set; }
        public string? TPAId { get; set; }
        public string? TPAName { get; set; }
        public string? TPAClaimReferenceNumber { get; set; }
        public string? NetworkProviderId { get; set; }
        public string? NetworkProviderName { get; set; }
        public string? NetworkProviderInvoiceNumber { get; set; }
        public string? ProcedureDate { get; set; }
        public string? ServiceCategoryId { get; set; }
        public string? ServiceCategoryName { get; set; }
        public string? ServiceName { get; set; }
        public string? CardNo { get; set; }
        public string? PatientName { get; set; }
        public string? DiagnosticCode { get; set; }
        public string? DiagnosticDescription { get; set; }
        public string? AdmissionDate { get; set; }
        public string? DischargeDate { get; set; }
        public string? TreatmentCountry { get; set; }
        public string? IsReimbursement { get; set; }
        public string? AmountClaimedOriginalCurrency { get; set; }
        public string? OriginalCurrencyCode { get; set; }
        public string? AmountClaimedPlanCurrency { get; set; }//من يحدده
        public string? PlanCurrencyCode { get; set; }
        public string? AmountApprovedOriginalCurrency { get; set; }
        public string? AmountApprovedPlanCurrency { get; set; }
        public string? CoPaymentOriginalCurrency { get; set; }
        public string? CoPaymentPlanCurrency { get; set; }
        public string? PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public string? PolicyId { get; set; }
        public string? PolicyName { get; set; }

    }
    public class ClaimInputModel
    {
        public string? ClaimNumber { get; set; }
        public DateTime ClaimDate { get; set; }
        public int TPAId { get; set; }
        public string? TPAName { get; set; }
        public string? TPAClaimReferenceNumber { get; set; }
        public int NetworkProviderId { get; set; }
        public string? NetworkProviderName { get; set; }
        public string? NetworkProviderInvoiceNumber { get; set; }
        public DateTime ProcedureDate { get; set; }
        public int ServiceCategoryId { get; set; }
        public string? ServiceCategoryName { get; set; }
        public string? ServiceName { get; set; }
        public string? CardNo { get; set; }
        public string? PatientName { get; set; }
        public string? DiagnosticCode { get; set; }
        public string? DiagnosticDescription { get; set; }
        public DateTime AdmissionDate { get; set; }
        public DateTime? DischargeDate { get; set; }
        public string? TreatmentCountry { get; set; }
        public bool IsReimbursement { get; set; }
        public decimal AmountClaimedOriginalCurrency { get; set; }
        public string? OriginalCurrencyCode { get; set; }
        public decimal AmountClaimedPlanCurrency { get; set; }
        public string? PlanCurrencyCode { get; set; }
        public decimal? AmountApprovedOriginalCurrency { get; set; }
        public decimal? AmountApprovedPlanCurrency { get; set; }
        public string? CoPaymentOriginalCurrency { get; set; }
        public string? CoPaymentPlanCurrency { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public string? Notes { get; set; }
        public int PolicyId { get; set; }
        public string? PolicyName { get; set; }
    }
}
