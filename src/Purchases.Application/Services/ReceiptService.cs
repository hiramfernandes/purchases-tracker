using Purchases.Domain.Contracts.Repos;
using Purchases.Domain.Contracts.Services;
using Purchases.Domain.Models;
using Purchases.Domain.Models.DTO.Receipt;

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

    public async Task CreteAsync(string url, DateTime receivedDate, CancellationToken cancellationToken)
    {
        var newReceipt = new Receipt()
        {
            Url = url,
            ReceivedDate = receivedDate,
            ProcessedDate = receivedDate,
            Processed = true
        };
        
        await _repository.CreateAsync(newReceipt, cancellationToken);
    }

    public async Task<IEnumerable<GetReceiptDto>> GetAllAsync(int pageSize, CancellationToken cancellationToken)
    {
        var topNReceipts = await _repository.GetAllAsync(pageSize, cancellationToken);

        var receiptsDto = topNReceipts.Select(MapFrom);

        return receiptsDto;
    }

    public async Task<Receipt> GetByIdAsync(string url, CancellationToken cancellationToken)
    {
        var receipt = await _repository.GetByIdAsync(url, cancellationToken);
        
        return receipt;
    }

    public async Task<IEnumerable<Receipt>> GetByStatusAsync(bool processed, CancellationToken cancellationToken)
    {
        var receipts = await _repository.GetByStatusAsync(processed, cancellationToken);
        
        return receipts;
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

    private GetReceiptDto MapFrom(Receipt receipt)
    {
        if (receipt == null)
            throw new ArgumentNullException("Invalid Receipt");

        return new GetReceiptDto()
        {
            Url = receipt.Url,
            Processed = receipt.Processed,
            ReceivedDate = receipt.ReceivedDate?.ToString("dd/MM/yyyy"),
            ProcessingMessage = receipt.ProcessingMessage,
        };
    }
}