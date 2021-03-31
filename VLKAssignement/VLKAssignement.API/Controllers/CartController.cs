using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using VLKAssignement.API.Models;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController:ControllerBase
    {
        private readonly ITransferCartService _transferCartService;
        private readonly IMapper _mapper;

        public CartController(ITransferCartService transferCartService, IMapper mapper)
        {
            _transferCartService = transferCartService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("user/{userId}/{status?}")]
        [SwaggerOperation(description: "Gets the transfers that the selected user has in his/her cart. The result can be filtered by status: Pending/Signed/Cancelled")]        
        [SwaggerResponse(400, "The userId provided is incorrect")]
        [SwaggerResponse(200)]
        public IActionResult Get(Guid userId, string status)
        {
            if(userId == Guid.Empty)
            {
                return BadRequest($"The {nameof(userId)} provided is incorrect");
            }
            var model = _mapper.Map<List<TransferModel>>(_transferCartService.GetTransfersByUserAndStatus(userId, status));
            return Ok(model);
        }
    }
}
