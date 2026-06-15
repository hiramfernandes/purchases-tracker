using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models;
using Purchases.Domain.Models.DTO.Merchant;

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

    public async Task<IEnumerable<GetMerchantDto>> GetAllAsync(int pageSize, CancellationToken cancellationToken)
    {
        var merchants = await _merchantRepository.GetAllAsync(pageSize, cancellationToken);

        return merchants.Select(MapFrom);
    }

    public async Task<GetMerchantDto?> GetAsync(string id, CancellationToken cancellationToken)
    {
        var merchant = await _merchantRepository.GetAsync(id, cancellationToken);
        var output = MapFrom(merchant);

        return output;
    }

    public Task RemoveAsync(string id, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(string id, Merchant merchant, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    private GetMerchantDto MapFrom(Merchant merchant)
    {
        return new GetMerchantDto()
        {
            LegalName =  merchant.LegalName,
            TradeName = merchant.TradeName,
            Cnpj = merchant.Cnpj,
            Address = merchant.Address,
        };
    }
}
