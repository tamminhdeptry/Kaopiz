
using CET.Domain.Enum;

namespace CET.Repository.Entity.Users
{
    public class UserEntity : BaseEntity
    {
        public string UserName { get; set; }
        public string DisplayName {  get; set; }
        public string Password { get; set; }
        public CType Type { get; set; }
        public CMasterData Status { get; set; }

    }
}
