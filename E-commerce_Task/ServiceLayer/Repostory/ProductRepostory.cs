using DomainLayer.Models;
using Microsoft.EntityFrameworkCore;
using RepostoryLayer;
using ServiceLayer.IRepostory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.Repostory
{
	public class ProductRepostory : IGeneric<Products>
	{
		private readonly AppDbContext _context;
		public ProductRepostory(AppDbContext context)
		{
			_context =context;
		}
		public async Task<Products> AddAsync(Products t)
		{
			await _context.products.AddAsync(t);
			await _context.SaveChangesAsync();
			return t;
		}

		public async Task<List<Products>> GetAllAsync()
		{
			return await _context.products.ToListAsync(); 
		}

		public async Task<Products> GetByDeleteAsync(int id)
		{
			var getDelete = await _context.products.FindAsync(id);
			if(getDelete== null)
			{
				return null;
			}
			_context.products.Remove(getDelete);
			await _context.SaveChangesAsync();
			return getDelete;
		}

		public async Task<Products> GetByIdAsync(int id)
		{
			return await _context.products.FindAsync(id);
		}

		public async Task<Products> GetByUpdateAysnc(Products t)
		{
			_context.products.Update(t);
			await _context.SaveChangesAsync();
			return t;
		}
	}
}
