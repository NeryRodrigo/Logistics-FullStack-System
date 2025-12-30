using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace LogisticsCore.Application.Features.Products.Comands.CreateProduct
{
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("{PropertyName} no puede estar vacío.")
                .MaximumLength(50).WithMessage("{PropertyName} no puede exceder 50 caracteres.");

            RuleFor(p => p.Sku)
                .NotEmpty().WithMessage("{PropertyName} es requerido.")
                .Length(3, 10).WithMessage("El SKU debe tener entre 3 y 10 caracteres.");

            RuleFor(p => p.Price)
                .GreaterThan(0).WithMessage("El precio debe ser mayor a 0.");

            RuleFor(p => p.Stock)
                .GreaterThanOrEqualTo(0).WithMessage("El stock no puede ser negativo.");
            RuleFor(p => p.CategoryId)
                .NotEmpty().WithMessage("La categoría es obligatoria.");
        }
    }
}
