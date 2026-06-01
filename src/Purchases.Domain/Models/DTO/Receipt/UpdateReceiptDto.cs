namespace Purchases.Domain.Models.DTO.Receipt
{
    public class UpdateReceiptDto
    {
        public string? Url { get; set; }
        public bool Processed { get; set; }
        public string? ProcessingMessage { get; set; }
    }
}
