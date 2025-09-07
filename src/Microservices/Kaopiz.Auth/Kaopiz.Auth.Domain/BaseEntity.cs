using Kaopiz.Shared.Contracts;

namespace Kaopiz.Auth.Infrastructure
{
    public abstract class BaseEntity
    {
        public Guid Id { get; private set; } = Guid.CreateVersion7();
        public Guid CreatedBy { get; set; }
        public DateTimeOffset Created { get; set; }
        public Guid? ModifiedBy { get; set; }
        public DateTimeOffset? Modified { get; set; }
        public CMasterData Status { get; set; }
    }
}
