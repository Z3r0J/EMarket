using EMarket.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EMarket.Core.Application.ViewModel.Advertising
{
    public class SaveAdvertisingViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage="Name is required")]
        [DataType(DataType.Text)]
        public string Name { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.Text)]
        public string Description { get; set; }
        [Required(ErrorMessage = "Description is required")]
        [DataType(DataType.Currency)]
        public double Price { get; set; }
        public string PrincipalPhoto { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Category is Required")]
        public int CategoryId { get; set; }
        public int UserId { get; set; }

        [Required(ErrorMessage ="Select minimum 1 to 4 photos")]
        [DataType(DataType.Upload)]
        public IFormFileCollection Photos { get; set; }

        public List<Category> Categories { get; set; }
        public List<Gallery> Gallery { get; set; }
    }
}
