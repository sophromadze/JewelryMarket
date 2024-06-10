using JewelryMarket.Entities;
using JewelryMarket.Models;

namespace JewelryMarket.Interfaces
{
    public interface IJewelryItemService
    {
        Task<JewelryItem> GetItemByIdAsync(int id);
        Task<IEnumerable<JewelryItem>> GeTAllItemsAsync();
        Task<JewelryItem> AddItemAsync(JewelryItem item);
        Task UpdateItemAsync(JewelryItem item, JewelryItem request);
        Task DeleteItemAsync(JewelryItem item);
    }
}
