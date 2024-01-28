// Ignore Spelling: TPA

using ClamManagement.Data.Entities.Commen;

namespace ClaimManagement.Data.Entities
{
    public class TPA : BaseEntity
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameCode { get; set; }
        public string? Email { get; set; }
        public string? PhoneLandLine { get; set; }
        public string? RepresentativeName { get; set; }
        public string? RepresentativeMobileNo {  get; set; }
        public string? RepresentativeEmail { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public bool? IsActive { get; set; }
        public int? DisplayOrder { get; set; }
        public ICollection<NetworkProvider>? NetworkProviders { get; set; }
        public ICollection<Claim>? Claims { get; set; }

    }
}
