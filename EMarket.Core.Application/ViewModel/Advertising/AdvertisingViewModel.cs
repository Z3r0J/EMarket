using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.ViewModel.Advertising
{
    public class AdvertisingViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public string PrincipalPhoto { get; set; }
        public string Category { get; set; }
        public string User { get; set; }

        public List<Gallery> Gallery {get;set;}
    }
}
