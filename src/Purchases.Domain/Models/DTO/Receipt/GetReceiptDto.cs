namespace Purchases.Domain.Models.DTO.Receipt
{
    public class GetReceiptDto
    {
        public string? Url { get; set; }
        public string? ReceivedDate { get; set; }
        public bool Processed { get; set; }
        public string? ProcessingMessage { get; set; }
    }
}
