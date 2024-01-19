using ClaimManagement.Data.Entities;

namespace ClaimManagement.Model
{
    public class NetworkProviderOutputModelSimple
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? NameCode { get; set; }
        public string? OfficialName { get; set; }
        public string? Email { get; set; }
        public string? PhoneLandLine { get; set; }
        public string? RepresentativeName { get; set; }
        public string? RepresentativeMobileNo { get; set; }
        public string? RepresentativeEmail { get; set; }
        public string? Country { get; set; }
        public string? City { get; set; }
        public string? Address { get; set; }
        public bool IsActive { get; set; }
        public int DisplayOrder { get; set; }

    }
    public class NetworkOProviderOutputModelDetailed : NetworkProviderOutputModelSimple
    {
        public ICollection<TPAOutputModelSimple>? TPAs { get; set; }
        public ICollection<ClaimOutputModelSimple>? Claims { get; set; }
    }
}
