using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainLayer.DTOS
{
	public class AddProductDto
	{
		public string Name { get; set; }
		

		public string Categories { get; set; }

		public int MinimumQuantity { get; set; }
		public decimal Price { get; set; }
		public decimal DiscountRate { get; set; }
		
	

	}
}
