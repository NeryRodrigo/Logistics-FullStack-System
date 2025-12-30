using LogisticsCore.Application.Features.Categories.Comands.CreateCategory;
using LogisticsCore.Application.Features.Categories.Queries.GetAllCategories;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace LogisticsCore.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoriesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> Create([FromBody] CreateCategoryCommand command)
        {
            return Ok(await _mediator.Send(command));
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            // El controlador solo dice: "Mediador, encárgate de esta query"
            var result = await _mediator.Send(new GetAllCategoryQuery());
            return Ok(result);
        }
    }
}
