using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models;

namespace Purchases.Application.Services;

public class MerchantService : IMerchantService
{
    private readonly IMerchantRepository _merchantRepository;

    public MerchantService(IMerchantRepository merchantRepository)
    {
        _merchantRepository = merchantRepository;
    }

    public async Task CreateAsync(Merchant newMerchant, CancellationToken cancellationToken)
    {
        await _merchantRepository.CreateAsync(newMerchant, cancellationToken);
    }

    public async Task<IEnumerable<Merchant>> GetAllAsync(int pageSize, CancellationToken cancellationToken)
    {
        var merchants = await _merchantRepository.GetAllAsync(pageSize, cancellationToken);

        return merchants;
    }

    public async Task<Purchase?> GetAsync(string id, CancellationToken cancellationToken)
    {
        var merchant = await GetAsync(id, cancellationToken);

        return merchant;
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
