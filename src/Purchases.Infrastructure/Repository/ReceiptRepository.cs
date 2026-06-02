using Microsoft.Extensions.Options;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Models;
using Purchases.Domain.Models.Settings;

namespace Purchases.Infrastructure.Repository;

public class ReceiptRepository : IReceiptRepository
{
    private readonly IMongoCollection<Receipt> _receiptsCollection;

    private readonly string _collectionName = "receipts";

    public ReceiptRepository(
        IOptions<MongoDbSettings> databaseSettings,
        IMongoClient mongoClient)
    {
        var dbName = databaseSettings.Value.DatabaseName;
        var mongoDatabase = mongoClient.GetDatabase(dbName);

        _receiptsCollection = mongoDatabase.GetCollection<Receipt>(_collectionName);
    }

    public async Task CreateAsync(Receipt newReceipt, CancellationToken cancellationToken)
    {
        InsertOneOptions options = new InsertOneOptions
        {
            BypassDocumentValidation = false,
            Comment = "Added using asp.net backend"
        };

        await _receiptsCollection.InsertOneAsync(newReceipt, options, cancellationToken);
    }

    public async Task<IEnumerable<Receipt>> GetAllAsync(int pageSize, CancellationToken cancellationToken)
    {
        var queryableCollection = _receiptsCollection.AsQueryable();
        return await queryableCollection
            .OrderByDescending(x => x.ReceivedDate)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task<Receipt?> GetByIdAsync(string url, CancellationToken cancellationToken)
    {
        var receipts = 
            await _receiptsCollection.FindAsync(
                receipt => receipt.Url == url, 
                cancellationToken: cancellationToken);

        return receipts.FirstOrDefault(cancellationToken);
    }
    
    public async Task<IEnumerable<Receipt>> GetByStatusAsync(bool processed, int pageSize, CancellationToken cancellationToken)
    {
        var queryableCollection = _receiptsCollection.AsQueryable();

        return await queryableCollection.Where(receipt => receipt.Processed == processed)
            .OrderByDescending(r => r.ReceivedDate)
            .Take(pageSize)
            .ToListAsync(cancellationToken);
    }

    public async Task UpdateStatusAsync(Receipt receiptToUpdate, CancellationToken cancellationToken)
    {
        await _receiptsCollection.ReplaceOneAsync(
            receipt => receipt.Url == receiptToUpdate.Url, 
            receiptToUpdate, 
            cancellationToken: cancellationToken);
    }

    public async Task DeleteAsync(string url, CancellationToken cancellationToken)
    {
        await _receiptsCollection.DeleteOneAsync(receipt => receipt.Url == url, cancellationToken);
    }
}