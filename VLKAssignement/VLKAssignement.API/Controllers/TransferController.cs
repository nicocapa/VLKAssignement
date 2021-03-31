using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VLKAssignement.API.Models;
using VLKAssignement.DataAccess.Models;
using VLKAssignement.Service.Interfaces;

namespace VLKAssignement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController:ControllerBase
    {
        private readonly ITransferService _transferService;
        private readonly IMapper _mapper;
        public TransferController(ITransferService transferService, IMapper mapper)
        {
            _transferService = transferService;
            _mapper = mapper;
        }

        [HttpPost]
        [SwaggerOperation(description: "Creates a transfer for a user")]
        [SwaggerResponse(400, "The model provided contains validation errors")]
        [SwaggerResponse(201)]
        public IActionResult Post([FromBody] TransferModel transfer)
        {
            var model = _mapper.Map<Transfer>(transfer);
            var newTransferId = _transferService.Add(model);
            return Created("api/transfer", newTransferId);
        }

        [HttpPost]
        [Route("sign")]
        [SwaggerOperation(description: "Signs the provided transfer")]
        [SwaggerResponse(400, "The transferId provided is in a wrong format")]
        [SwaggerResponse(409, "There are validations errors")]
        [SwaggerResponse(201)]
        public async Task<IActionResult> Sign([FromBody] Guid transferId)
        {

            var result = await _transferService.Sign(transferId);
            if (result.ValidationResult.Succeded)
            {
                return Created("api/transaction", result.TransactionId);
            }
            return Conflict(string.Join(", ", result.ValidationResult.Messages));
        }
    }
}
