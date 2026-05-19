using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models;

namespace Purchases.Application.Services;

public class ReceiptService : IReceiptService
{
    private readonly IReceiptRepository _repository;

    public ReceiptService(IReceiptRepository repository)
    {
        _repository = repository;
    }
    
    public async Task CreteAsync(string url, CancellationToken cancellationToken)
    {
        var newReceipt = new Receipt()
        {
            Url = url,
            ReceivedDate = DateTime.UtcNow,
            Processed = false
        };

        await _repository.CreateAsync(newReceipt, cancellationToken);
    }

    public async Task<Receipt> GetByIdAsync(string url, CancellationToken cancellationToken)
    {
        var receipt = await _repository.GetByIdAsync(url, cancellationToken);
        
        return receipt;
    }

    public async Task UpdateStatusAsync(
        string url,
        bool processed,
        DateTime processingDate,
        CancellationToken cancellationToken)
    {
        var receiptFromDb = await _repository.GetByIdAsync(url, cancellationToken);
        
        if (receiptFromDb == null)
            throw new InvalidOperationException($"Receipt with url '{url}' was not found.");
      
        receiptFromDb.Processed = processed;
        receiptFromDb.ProcessedDate = processingDate;
        
        await _repository.UpdateStatusAsync(receiptFromDb, cancellationToken);
    }
}