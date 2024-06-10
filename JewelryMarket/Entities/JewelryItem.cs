using JewelryMarket.enums;

namespace JewelryMarket.Entities
{
    public class JewelryItem : BaseEntity
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public JewelryCategory Category { get; set; }
        public int Quantity { get; set; }
    }
}
