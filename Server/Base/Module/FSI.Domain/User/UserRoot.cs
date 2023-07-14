using Volo.Abp.Domain.Entities.Auditing;

namespace FSI.Domain.User
{
    public class UserRoot : FullAuditedAggregateRoot<Guid>
    {
        public string Name { get; set; }

        public string Phone { get; set; }

        public DateTime DateOfBirth { get; set; }

        public string? IdentityCard { get; set; }

        public int? Location { get; set; }

        public string? WorkingPlace { get; set; }

        public Guid AccountId { get; set; }

        public Account.Account Account { get; set; }

        public string? AvatarUrl { get; set; }

        public bool? IsNewProfile { get; set; }

        public int? Job { get; set; }

        public bool? Gender { get; set; }
    }
}
