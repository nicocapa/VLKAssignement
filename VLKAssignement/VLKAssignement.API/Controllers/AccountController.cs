using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;
        
        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;     
        }

        [HttpGet]
        [Route("user/{userId}")]
        [SwaggerOperation(description: "Gets the account for the selected user")]
        [SwaggerResponse(400, "The userId provided is incorrect")]
        [SwaggerResponse(400, "The selected user does not have an account")]
        [SwaggerResponse(200)]
        public IActionResult Get(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest($"The {nameof(userId)} provided is incorrect");
            }
            var account = _accountService.GetByUserId(userId);            
            if(account == null)
            {
                return BadRequest("The selected user does not have an account");
            }
            return Ok(account);
        }
    }
}
