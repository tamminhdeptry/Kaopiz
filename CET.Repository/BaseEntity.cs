using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CET.Repository
{
    public class BaseEntity
    {
        public Guid Id { get; set; }
        public Guid CreatedBy {  get; set; }
        public DateTimeOffset Created {  get; set; }
        public Guid ModifiedBy { get; set; }
        public DateTimeOffset Modified { get; set; }

    }
}
