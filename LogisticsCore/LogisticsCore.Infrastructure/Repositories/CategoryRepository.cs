using LogisticsCore.Application.Contract.Persistence;
using LogisticsCore.Domain.Entities;
using LogisticsCore.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly LogisticsDbContext _context;

        public CategoryRepository(LogisticsDbContext context)
        {
            _context = context;
        }

        public async Task<Category> AddAsync(Category entity)
        {
            await _context.Categories.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<IEnumerable<Category>> GetAllAsync()
        {
            return await _context.Categories.ToListAsync();
        }
    }
}
