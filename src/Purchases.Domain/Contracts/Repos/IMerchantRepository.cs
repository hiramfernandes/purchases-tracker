using Purchases.Domain.Models;

namespace Purchases.Domain.Contracts.Repos;

public interface IMerchantRepository
{
    Task CreateAsync(Merchant newMerchant, CancellationToken cancellationToken);
    Task<IEnumerable<Merchant>> GetAllAsync(int pageSize, CancellationToken cancellationToken);
    Task<Merchant?> GetAsync(string cnpj, CancellationToken cancellationToken);
    Task RemoveAsync(string cnpj, CancellationToken cancellationToken);
    Task UpdateAsync(string cnpj, Merchant updatedMerchant, CancellationToken cancellationToken);
}
