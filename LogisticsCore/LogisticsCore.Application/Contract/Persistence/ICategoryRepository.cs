using LogisticsCore.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Application.Contract.Persistence
{
    public interface ICategoryRepository
    {
        Task<Category> AddAsync(Category entity);
        Task<IEnumerable<Category>> GetAllAsync();
    }
}
