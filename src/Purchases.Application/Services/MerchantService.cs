using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models;

namespace Purchases.Application.Services;

public class MerchantService : IMerchantService
{
    public MerchantService()
    {
    }

    public Task CreateAsync(Merchant newMerchant, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Merchant>> GetAllAsync(int pageSize, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Purchase?> GetAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<Purchase> GetByUrlAsync(string url, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(string id, Merchant merchant, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
