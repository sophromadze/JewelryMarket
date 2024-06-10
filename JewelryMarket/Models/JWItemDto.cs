namespace JewelryMarket.Models
{
    public class JWItemDto
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public JewelryCategory Category { get; set; }
    }
}
