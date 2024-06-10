using JewelryMarket.Entities;

namespace JewelryMarket.Interfaces
{
    public interface IJewelryItemRepository
    {
        Task<JewelryItem> GetItemByIdAsync(int id);
        Task<IEnumerable<JewelryItem>> GeTAllItemsAsync();
        Task<JewelryItem> AddItemAsync(JewelryItem item);
        Task UpdateItemAsync(JewelryItem item);
        Task DeleteItemAsync(JewelryItem item);
    }
}
