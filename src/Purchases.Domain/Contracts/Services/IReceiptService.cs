using Purchases.Domain.Models;
using Purchases.Domain.Models.DTO.Receipt;

namespace Purchases.Domain.Contracts.Services;

public interface IReceiptService
{
    Task CreteAsync(string url, CancellationToken cancellationToken);
    Task CreteAsync(string url, DateTime receivedDate, CancellationToken cancellationToken);
    Task<IEnumerable<GetReceiptDto>> GetAllAsync(int pageSize, CancellationToken cancellationToken);
    Task<Receipt> GetByIdAsync(string url, CancellationToken cancellationToken);
    Task<IEnumerable<Receipt>> GetByStatusAsync(bool processed, int pageSize, CancellationToken cancellationToken);
    Task UpdateStatusAsync(string url, bool processed, DateTime? processingDate, CancellationToken cancellationToken);
}
