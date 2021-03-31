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
    public class TransactionController : ControllerBase
    {
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransactionController(ITransactionService transactionService, IMapper mapper)
        {
            _transactionService = transactionService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("user/{userId}")]
        [SwaggerOperation(description: "Gets the transactions for the selected user")]
        [SwaggerResponse(400, "The userId provided is incorrect")]        
        [SwaggerResponse(200)]
        public IActionResult Get(Guid userId)
        {
            if (userId == Guid.Empty)
            {
                return BadRequest($"The {nameof(userId)} provided is incorrect");
            }
            var transactions = _transactionService.GetByUserId(userId);  
            
            return Ok(_mapper.Map<List<TransactionModel>>(transactions));
        }
    }
}
