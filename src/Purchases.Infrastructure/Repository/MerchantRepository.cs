using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Models;
using Purchases.Domain.Models.Settings;

namespace Purchases.Infrastructure.Repository;

public class MerchantRepository : IMerchantRepository
{
    private readonly IMongoCollection<Merchant> _merchantCollection;

    private readonly string _collectionName = "merchants";

    public MerchantRepository(
        IOptions<MongoDbSettings> databaseSettings,
        IMongoClient mongoClient)
    {
        var dbName = databaseSettings.Value.DatabaseName;
        var mongoDatabase = mongoClient.GetDatabase(dbName);

        _merchantCollection = mongoDatabase.GetCollection<Merchant>(_collectionName);
    }

    public async Task<IEnumerable<Merchant>> GetAllAsync(int pageSize, CancellationToken cancellationToken)
    {
        var queryableCollection = _merchantCollection.AsQueryable();
        return await queryableCollection
            .OrderByDescending(x => x.TradeName)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Merchant?> GetAsync(string cnpj, CancellationToken cancellationToken) =>
        await _merchantCollection.Find(x => x.Cnpj == cnpj).FirstOrDefaultAsync(cancellationToken);

    public async Task CreateAsync(Merchant newMerchant, CancellationToken cancellationToken)
    {
        InsertOneOptions options = new InsertOneOptions
        {
            BypassDocumentValidation = false,
            Comment = "Added using asp.net backend"
        };

        await _merchantCollection.InsertOneAsync(newMerchant, options, cancellationToken);
    }

    public async Task UpdateAsync(string cnpj, Merchant updatedMerchant, CancellationToken cancellationToken) =>
        await _merchantCollection.ReplaceOneAsync(x => x.Cnpj == cnpj, updatedMerchant);


    public async Task UpdateStatusAsync(string cnpj, Merchant updatedMerchant, CancellationToken cancellationToken)
    {
        await _merchantCollection.ReplaceOneAsync(
            merchant => merchant.Cnpj == cnpj,
            updatedMerchant,
            cancellationToken: cancellationToken);
    }

    public async Task RemoveAsync(string cnpj, CancellationToken cancellationToken) =>
        await _merchantCollection.DeleteOneAsync(x => x.Cnpj == cnpj, cancellationToken: cancellationToken);
}
