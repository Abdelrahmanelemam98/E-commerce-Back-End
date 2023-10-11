using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace DomainLayer.DTOS
{
	public class UpdateProductDtos
	{
		public string Name { get; set; }
		public string Categories { get; set; }

		public int MinimumQuantity { get; set; }
		public decimal Price { get; set; }
		public decimal DiscountRate { get; set; }
		
	}
}
