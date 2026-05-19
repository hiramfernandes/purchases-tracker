using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Models;
using Purchases.Domain.Models.Settings;

namespace Purchases.Infrastructure.Repository;

public class ReceiptRepository : IReceiptRepository
{
    private readonly string _collectionName = "receipts";

    private readonly IMongoCollection<Receipt> _receiptsCollection;

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

    public async Task<Receipt?> GetByIdAsync(string url, CancellationToken cancellationToken)
    {
        var receipt = 
            await _receiptsCollection.FindAsync(
                receipt => receipt.Url == url, 
                cancellationToken: cancellationToken);
        
        return receipt.FirstOrDefault(cancellationToken);
    }

    public async Task UpdateStatusAsync(Receipt receiptToUpdate, CancellationToken cancellationToken)
    {
        await _receiptsCollection.ReplaceOneAsync(
            receipt => receipt.Url == receiptToUpdate.Url, 
            receiptToUpdate, 
            cancellationToken: cancellationToken);
    }
}