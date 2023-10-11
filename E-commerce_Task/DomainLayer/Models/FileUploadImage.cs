using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Http;

namespace DomainLayer.Models
{
	public class FileUploadImage
	{
		public IFormFile File { get; set; }
	}
}
