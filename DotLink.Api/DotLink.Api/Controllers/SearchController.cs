using DotLink.Application.Features.Search;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DotLink.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SearchController : ControllerBase
    {
        private readonly IMediator _mediator;
        public SearchController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> Search([FromQuery] CompositeSearchQuery query)
        {
            var results = await _mediator.Send(query);
            return Ok(results);
        }
    }
}
