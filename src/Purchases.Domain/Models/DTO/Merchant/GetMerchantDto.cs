namespace Purchases.Domain.Models.DTO.Merchant;

public class GetMerchantDto
{
    public string? Cnpj { get; set; }
    public string? LegalName { get; set; }
    public string? TradeName { get; set; }
    public Address? Address { get; set; }
}
