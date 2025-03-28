using Common.Domains.Entities;
using Dapper.Contrib.Extensions;
using Domain.Enums;

namespace DotNetTraining.Domains.Entities
{
    [Table("users")]
    public class User : SystemLogEntity<Guid>
    {
        public string Roles { get; set; } = "GUEST";
        public string UserName { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; }
        public string FullName => $"{LastName} {FirstName}";
        public string Email { get; set; }
        public string Password { get; set; }

        public UserStatus Status { get; set; } = UserStatus.Active;

        public DateTime? LastLoggedIn { get; set; }
    }
}
