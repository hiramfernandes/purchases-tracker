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
                        // Get unprocessed records (all at once)
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
                            var existingReceipt = await _purchaseService.GetByUrlAsync(unprocessedReceipt.Url, stoppingToken);

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
                            
                            await _receiptRetrieverService.HandleReceiptUrl(unprocessedReceipt.Url, default, stoppingToken);
                            await _receiptService.UpdateStatusAsync(unprocessedReceipt.Url, true, DateTime.UtcNow, stoppingToken);
                        }
                    }
                    catch (Exception exc)
                    {
                        _logger.LogError(exc, $"Error executing worker: {exc.Message}");
                    }

                    // Edge cases:
                    // - Look for duplicates
                    // - Look for inconsistent data (manual input vs LLM reported)
                    // - Save items into a dedicated container??
                }

                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}