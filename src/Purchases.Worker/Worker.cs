using System.Diagnostics;
using Purchases.Domain.Contracts.Services;

namespace Purchases.Worker
{
    public class Worker : BackgroundService
    {
        private readonly IPurchaseService _purchaseService;
        private readonly IReceiptService _receiptService;
        private readonly IReceiptRetrieverService _receiptRetrieverService;
        private readonly ILogger<Worker> _logger;

        public Worker(
            IServiceScopeFactory scopeFactory,
            ILogger<Worker> logger)
        {
            using IServiceScope scope = scopeFactory.CreateScope();

            _purchaseService = scope.ServiceProvider.GetRequiredService<IPurchaseService>();
            _receiptService = scope.ServiceProvider.GetRequiredService<IReceiptService>();
            _receiptRetrieverService = scope.ServiceProvider.GetRequiredService<IReceiptRetrieverService>();
            _logger = scope.ServiceProvider.GetRequiredService<ILogger<Worker>>();
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                    try
                    {
                        // Retrieve unprocessed records (all at once)
                        var unprocessedReceipts = await _receiptService.GetByStatusAsync(
                            processed: false,
                            cancellationToken: stoppingToken);

                        foreach (var unprocessedReceipt in unprocessedReceipts)
                        {
                            Debug.WriteLine($"URL: {unprocessedReceipt.Url}");
                            Debug.WriteLine($"Processed: {unprocessedReceipt.Processed}");
                            Debug.WriteLine($"Processing Date: {unprocessedReceipt.ProcessedDate}");
                            Debug.WriteLine($"Received Date: {unprocessedReceipt.ReceivedDate}");

                            // Validate
                            // Purchases record already exists
                            var existingReceipt =
                                await _purchaseService.GetByUrlAsync(unprocessedReceipt.Url, stoppingToken);

                            // Validate
                            // If exists does it properly contain Items
                            if (existingReceipt is { Items.Length: > 0 })
                            {
                                // Mark it as processed
                                await _receiptService.UpdateStatusAsync(
                                    url: unprocessedReceipt.Url,
                                    processed: true,
                                    processingDate: DateTime.UtcNow,
                                    cancellationToken: stoppingToken);

                                continue;
                            }

                            await _receiptRetrieverService.HandleReceiptUrl(unprocessedReceipt.Url, default,
                                stoppingToken);
                            await _receiptService.UpdateStatusAsync(unprocessedReceipt.Url, true, DateTime.UtcNow,
                                stoppingToken);
                        }
                    }
                    catch (Exception exc)
                    {
                        _logger.LogError(exc, $"Error executing worker: {exc.Message}");
                    }

                    var purchases = await _purchaseService.GetAllAsync(
                        pageSize: 10000, 
                        cancellationToken: stoppingToken);

                    // - Iterate through existing purchase records
                    foreach (var purchase in purchases)
                    {
                        // - Look for inconsistent data (manual input vs LLM reported)
                        if (string.IsNullOrWhiteSpace(purchase.PurchaseUrl))
                        {
                            // Remove the record due to lack of relevant data
                            await _purchaseService.RemoveAsync(purchase.Id!);
                            
                            Debug.WriteLine($"Purchase removed: {purchase.Id} ({purchase.PurchaseDate})");
                            
                            continue;
                        }
                        
                        // See if corresponding receipt exists
                        var existingReceipt = await _receiptService.GetByIdAsync(purchase.PurchaseUrl, stoppingToken);

                        if (existingReceipt == null)
                        {
                            await _receiptService.CreteAsync(
                                purchase.PurchaseUrl, 
                                purchase.PurchaseDate ?? DateTime.UtcNow, 
                                stoppingToken);
                            
                            Debug.WriteLine($"Receipt created: {purchase.Id} ({purchase.PurchaseDate})");
                        }
                    }

                    // Edge cases:
                    // - Look for duplicates
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}