using Purchases.Domain.Models;

namespace Purchases.Domain.Contracts.Services;

public interface IReceiptService
{
    Task CreteAsync(string url, CancellationToken cancellationToken);
    Task<Receipt> GetByIdAsync(string url, CancellationToken cancellationToken);
    Task UpdateStatusAsync(string url, bool processed, DateTime processingDate, CancellationToken cancellationToken);
}
