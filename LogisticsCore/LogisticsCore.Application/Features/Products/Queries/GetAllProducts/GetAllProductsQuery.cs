using LogisticsCore.Application.Contract.Persistence;
using LogisticsCore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Application.Features.Products.Queries.GetAllProducts
{
    // 1. La Query: En este caso no pide filtros, así que está vacía.
    // Devuelve una lista de productos.
    public class GetAllProductsQuery : IRequest<IEnumerable<Product>>
    {
    }

    // 2. El Handler
    public class GetAllProductsQueryHandler : IRequestHandler<GetAllProductsQuery, IEnumerable<Product>>
    {
        private readonly IProductRepository _repository;

        public GetAllProductsQueryHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<Product>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync();
        }
    }
}
