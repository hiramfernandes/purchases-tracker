using Microsoft.AspNetCore.Mvc;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models;

namespace aspnet_mongo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MerchantsController : ControllerBase
    {
        private readonly IMerchantService _merchantService;

        public MerchantsController(IMerchantService merchantService)
        {
            _merchantService = merchantService;
        }

        [HttpGet]
        [Produces(typeof(Merchant))]
        public async Task<IActionResult> GetMerchant([FromRoute] string id, CancellationToken cancellationToken)
        {
            var merchant = await _merchantService.GetAsync(id, cancellationToken);

            return Ok(merchant);
        }

        [HttpPost]
        public async Task<IActionResult> CreateMerchant([FromBody] Merchant merchant, CancellationToken cancellationToken)
        {
            await _merchantService.CreateAsync(merchant, cancellationToken);

            return NoContent();
        }
    }
}
