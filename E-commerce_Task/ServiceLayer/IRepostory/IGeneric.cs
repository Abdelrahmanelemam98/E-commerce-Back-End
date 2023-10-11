using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLayer.IRepostory
{
	public interface IGeneric<T> where T : class
	{
		Task<List<T>> GetAllAsync();
		Task<T> GetByIdAsync(int id);
		Task<T> GetByDeleteAsync(int id);
		Task<T> GetByUpdateAysnc(T t);
		Task<T> AddAsync(T t);
	}
}
