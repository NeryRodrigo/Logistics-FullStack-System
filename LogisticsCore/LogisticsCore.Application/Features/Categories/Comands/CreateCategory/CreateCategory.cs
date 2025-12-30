using FluentValidation;
using LogisticsCore.Application.Contract.Persistence;
using LogisticsCore.Domain.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogisticsCore.Application.Features.Categories.Comands.CreateCategory
{
    // 1. Comando (DTO)
    public class CreateCategoryCommand : IRequest<Guid>
    {
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
    }

    // 2. Validador
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(p => p.Name)
                .NotEmpty().WithMessage("El nombre de la categoría es obligatorio.")
                .MaximumLength(50);
        }
    }

    // 3. Handler (Lógica)
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Guid>
    {
        private readonly ICategoryRepository _repository;

        public CreateCategoryCommandHandler(ICategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category(request.Name, request.Description);
            await _repository.AddAsync(category);
            return category.Id;
        }
    }
}
