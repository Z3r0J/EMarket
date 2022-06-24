using EMarket.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.ViewModel.Advertising
{
    public class DetailsAdvertisingViewModel
    {
        public int Id { get; set; }
        public List<Gallery> Gallery { get; set; }
        public double Price { get; set; }
        public string Category { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string PublishedBy { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public DateTime PublicationDate { get; set; }
    }
}
