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

        [HttpGet("{id}")]
        [Produces(typeof(Merchant))]
        public async Task<IActionResult> GetMerchant([FromRoute] string id, CancellationToken cancellationToken)
        {
            var merchant = await _merchantService.GetAsync(id, cancellationToken);

            return Ok(merchant);
        }

        [HttpGet]
        public async Task<IActionResult> GetMerchants(CancellationToken cancellationToken)
        {
            var merchants = await _merchantService.GetAllAsync(20,  cancellationToken);
            
            return Ok(merchants);
        }


        [HttpPost]
        public async Task<IActionResult> CreateMerchant([FromBody] Merchant merchant, CancellationToken cancellationToken)
        {
            await _merchantService.CreateAsync(merchant, cancellationToken);

            return NoContent();
        }
    }
}
