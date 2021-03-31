using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        
        public UserController(IUserService userService)
        {
            _userService = userService;     
        }

        [HttpGet]        
        [SwaggerOperation(description: "Gets the users in the system")]                
        [SwaggerResponse(200)]
        public IActionResult Get()
        {
            return Ok(_userService.GetAll());
        }
    }
}
