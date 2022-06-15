using EMarket.Core.Domain.Common;
using System.Collections.Generic;

namespace EMarket.Core.Domain.Entities
{
    public class Advertising : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }

        public int CategoryId { get; set; }
        public Category Categories { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public ICollection<Gallery> Gallery { get; set; }
    }
}
