using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models.DTO.Receipt;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace aspnet_mongo.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReceiptsController : ControllerBase
{
    private readonly IReceiptService _receiptService;

    public ReceiptsController(IReceiptService receiptService)
    {
        _receiptService = receiptService;
    }

    [HttpGet]
    [Produces(typeof(IEnumerable<GetReceiptDto>))]
    public async Task<IActionResult> GetReceipts(CancellationToken cancellationToken)
    {
        var pageSize = 80;

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

    [HttpGet("status")]
    [Produces(typeof(IEnumerable<GetReceiptDto>))]
    public async Task<IActionResult> GetReceiptsByStatusAsync(bool processed, CancellationToken cancellationToken)
    {
        var pageSize = 80;

        var results = await _receiptService.GetByStatusAsync(
            processed,
            pageSize,
            cancellationToken);

        return Ok(results);
    }

    [HttpGet("url")]
    [Produces(typeof(GetReceiptDto))]
    public async Task<IActionResult> GetByUrl([FromQuery] string url, CancellationToken cancellationToken)
    {
        var receipt = await _receiptService.GetByIdAsync(url, cancellationToken);

        return Ok(receipt);
    }

    [HttpPut]
    public async Task<IActionResult> UpdateReceipt(UpdateReceiptDto updatedReceipt, CancellationToken cancellationToken)
    {
        await _receiptService.UpdateStatusAsync(
            updatedReceipt.Url!,
            updatedReceipt.Processed,
            DateTime.UtcNow,
            updatedReceipt.ProcessingMessage,
            cancellationToken);

        return Ok();
    }
}