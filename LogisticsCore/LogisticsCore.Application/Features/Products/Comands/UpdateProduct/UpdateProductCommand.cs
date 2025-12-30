using LogisticsCore.Application.Contract.Persistence;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Application.Features.Products.Comands.UpdateProduct
{
    public class UpdateProductCommand : IRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Sku { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public Guid CategoryId { get; set; }
    }

    // 2. Handler
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly IProductRepository _repository;

        public UpdateProductCommandHandler(IProductRepository repository)
        {
            _repository = repository;
        }

        public async Task Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            // A. Buscar el producto
            var productToUpdate = await _repository.GetByIdAsync(request.Id);

            // B. Validar que exista (Patrón Guard Clause)
            if (productToUpdate == null)
            {
                // En un proyecto real lanzarías una NotFoundException custom
                throw new Exception($"Product {request.Id} not found");
            }

            // C. Actualizar usando el método del Dominio (DDD)
            productToUpdate.UpdateDetails(request.Name, request.Sku, request.Price, request.CategoryId);

            // D. Guardar cambios
            await _repository.UpdateAsync(productToUpdate);
        }
    }
}
