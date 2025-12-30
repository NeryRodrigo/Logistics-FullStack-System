using LogisticsCore.Application.Features.Products.Comands.CreateProduct;
using LogisticsCore.Application.Features.Products.Comands.UpdateProduct;
using LogisticsCore.Application.Features.Products.Queries.GetAllProducts;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            // El controlador solo dice: "Mediador, encárgate de esta query"
            var result = await _mediator.Send(new GetAllProductsQuery());
            return Ok(result);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateProductCommand command)
        {
            // El controlador solo dice: "Mediador, encárgate de este comando"
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Update(Guid id, [FromBody] UpdateProductCommand command)
        {
            if (id != command.Id)
            {
                command.Id = id; // Aseguramos que el ID del body coincida con la URL
            }

            await _mediator.Send(command);
            return NoContent(); // 204: Todo salió bien, no hay contenido que devolver
        }
    }
}
