using LogisticsCore.Application.Contract.Persistence;
using LogisticsCore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Application.Features.Categories.Queries.GetAllCategories
{
    // 1. La Query: En este caso no pide filtros, así que está vacía.
    // Devuelve una lista de productos.
    public class GetAllCategoryQuery : IRequest<IEnumerable<Category>>
    {
    }

    // 2. El Handler
    public class GetAllCategoryQueryHandler : IRequestHandler<GetAllCategoryQuery, IEnumerable<Category>>
    {
        private readonly ICategoryRepository _repository;

        public GetAllCategoryQueryHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Category>> Handle(GetAllCategoryQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
