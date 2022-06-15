using EMarket.Core.Domain.Common;
using System.Collections.Generic;

namespace EMarket.Core.Domain.Entities
{
    public class Category : AuditableBaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }

        public ICollection<Advertising> Advertisings {get;set;}
    }
}
