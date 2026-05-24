using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models.DTO.Receipt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace aspnet_mongo.Controllers;

[Route("api/[controller]")]
[ApiController]
[AllowAnonymous]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService _receiptService;

    public ReceiptsController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpGet]
    [Produces(typeof(List<GetReceiptDto>))]
    public async Task<IActionResult> GetReceipts(CancellationToken cancellationToken)
    {
        // TODO: Move to appsettings
        var pageSize = 50;

        try
        {
            var receipts = await _receiptService.GetAllAsync(pageSize, cancellationToken);

            return Ok(receipts);
        }
        catch (Exception exc)
        {
            return BadRequest(exc.Message);
        }
    }
}
