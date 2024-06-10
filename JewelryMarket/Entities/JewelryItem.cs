using JewelryMarket.Models;

namespace JewelryMarket.Entities
{
    public class JewelryItem : BaseEntity
    {
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public JewelryCategory Category { get; set; }
    }
}
