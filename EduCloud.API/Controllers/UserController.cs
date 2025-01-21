using EduCloud.Application.Users.Commands;
using EduCloud.Application.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EduCloud.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;

        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserCommand command)
        { 
            var response = await _mediator.Send(command);
            return Ok(response);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById([FromQuery] GetUserQuery getUserQuery)
        { 
            var reponse = await _mediator.Send(getUserQuery);
            return Ok(reponse);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var query = new GetAllUsersQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserCommand command)
        { 
            var response = await _mediator.Send(command);
            return Ok(response);     
        }
    }
}
