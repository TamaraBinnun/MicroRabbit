using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace MicroRabbit.Transfer.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        private readonly ILogger<TransferController> _logger;

        public TransferController(ILogger<TransferController> logger,
                                ITransferService transferService)
        {
            _logger = logger;
            _transferService = transferService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<AccountTransfer>> Get()
        {
            return Ok(_transferService.GetTransfers());
        }
    }
}