using Purchases.Domain.Models;
using Purchases.Domain.Models.DTO.Merchant;

namespace Purchases.Domain.Contracts.Services;

public interface IMerchantService
{
    Task CreateAsync(Merchant newMerchant, CancellationToken cancellationToken);
    Task<IEnumerable<GetMerchantDto>> GetAllAsync(int pageSize, CancellationToken cancellationToken);
    Task<GetMerchantDto?> GetAsync(string id, CancellationToken cancellationToken);
    Task RemoveAsync(string id, CancellationToken cancellationToken);
    Task UpdateAsync(string id, Merchant merchant, CancellationToken cancellationToken);
}