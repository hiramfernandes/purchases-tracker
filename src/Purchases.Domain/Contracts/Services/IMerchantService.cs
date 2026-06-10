using Purchases.Domain.Models;

namespace Purchases.Domain.Contracts.Services;

public interface IMerchantService
{
    Task CreateAsync(Merchant newMerchant, CancellationToken cancellationToken);
    Task<IEnumerable<Merchant>> GetAllAsync(int pageSize, CancellationToken cancellationToken);
    Task<Purchase?> GetAsync(string id, CancellationToken cancellationToken);
    Task RemoveAsync(string id, CancellationToken cancellationToken);
    Task UpdateAsync(string id, Merchant merchant, CancellationToken cancellationToken);
}
