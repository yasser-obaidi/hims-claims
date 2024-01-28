using ClamManagement.Data.Entities.Commen;
using System.Numerics;

namespace ClaimManagement.Services.PolicyManagement;

public class PolicyModel
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public IQueryable<PlanModel>? Plans { get; set; }
}
public class PlanModel 
{
    public int? Id { get; set; }
    public string? Name { get; set; }
    public decimal CoverageLimit { get; set; }
    public string? CoverageRegion { get; set; }
    public IQueryable<BenefitModel>? Benefits { get; set; }
    public string CurrencyCode { get; set; }
    public string AlternativeName { get; set; }
    public bool IsActive { get; set; }
}

public class CategoryModel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public IQueryable<CategoryModel>? SubCategories { get; set; }

}

public class BenefitModel 
{
    public int? Id { get; set; }
   // public int CategoryId { get; set; }
    public CategoryModel Category { get; set; }
    public string? Description { get; set; }
    //public int BenefitTypeId { get; set; }
    public BenefitTypeModel BenefitType { get; set; }
    public float MemberCoPaymentPercentage { get; set; }
    //public int BenefitRuleId { get; set; }
    public BenefitRuleModel BenefitRule { get; set; }



}

public class BenefitTypeModel 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
}

public class BenefitRuleModel 
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public LimitType LimitType { get; set; }

}

public enum LimitType
{
    None = 0,
    Monetary,
    Visit
}