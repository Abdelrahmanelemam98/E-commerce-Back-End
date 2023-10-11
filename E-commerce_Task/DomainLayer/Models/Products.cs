using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.Models
{
	public class Products
	{
		[Key]
		[Required,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int ProductCode { get; set; }
		[Required,MaxLength(200)]
		public string Name { get; set; }
		[Required, MaxLength(200)]

		public string Categories { get; set; }
		[Required, MaxLength(200)]

		public string image { get; set; }
		[Required, MaxLength(200)]

		public int MinimumQuantity { get; set; }
		[Required, MaxLength(200)]

		public decimal Price { get; set; }
		[Required, MaxLength(200)]

		public decimal DiscountRate { get; set; }
	}
}
