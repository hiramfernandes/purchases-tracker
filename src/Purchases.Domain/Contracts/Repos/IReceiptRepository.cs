using Purchases.Domain.Models;

namespace Purchases.Domain.Contracts.Repos;

public interface IReceiptRepository
{
    Task CreateAsync(Receipt newReceipt, CancellationToken cancellationToken);
    Task<IEnumerable<Receipt>> GetAllAsync(int pageSize, CancellationToken cancellationToken);
    Task<Receipt?> GetByIdAsync(string url, CancellationToken cancellationToken);
    Task<IEnumerable<Receipt>> GetByStatusAsync(bool processed, int pageSize, CancellationToken cancellationToken);
    Task UpdateStatusAsync(Receipt receiptToUpdate, CancellationToken cancellationToken);
}