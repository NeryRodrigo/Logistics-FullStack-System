using LogisticsCore.Application.Contract.Persistence;
using LogisticsCore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Application.Features.Products.Comands.CreateProduct
{
    // 1. El Comando: Es solo un DTO que transporta los datos del usuario.
    // Devuelve un Guid (el ID del producto creado).
    public class CreateProductCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public int Stock { get; set; }

        public Guid CategoryId { get; set; }
    }

    // 2. El Handler: Es quien recibe la orden y hace el trabajo sucio.
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly IProductRepository _repository;

        public CreateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Convertimos el comando a la Entidad de Dominio
            var productEntity = new Product(request.Name, request.Sku, request.Price, request.Stock, request.CategoryId);

            // Usamos el repositorio para guardar
            await _repository.AddAsync(productEntity);

            // Retornamos el ID generado
            return productEntity.Id;
        }
    }
}
