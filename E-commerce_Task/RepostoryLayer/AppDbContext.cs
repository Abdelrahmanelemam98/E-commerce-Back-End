using DomainLayer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepostoryLayer
{
    public class AppDbContext:IdentityDbContext<IdentityUser>
	{
		public AppDbContext()
		{

		}
		public AppDbContext( DbContextOptions<AppDbContext> options) : base(options)
		{

		}
		public DbSet<Products> products { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder.UseSqlServer("Server=.;Database=E-commerce_2;Trusted_Connection=True;Integrated Security=True ;TrustServerCertificate=True");
		}

	}
}
