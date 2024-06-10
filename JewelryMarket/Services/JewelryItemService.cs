using JewelryMarket.Entities;
using JewelryMarket.Interfaces;
using JewelryMarket.Models;

namespace JewelryMarket.Services;

public class JewelryItemService : IJewelryItemService
{
    private readonly IJewelryItemRepository _itemRepository;

    public JewelryItemService(IJewelryItemRepository itemRepository)
    {
        _itemRepository = itemRepository;
    }

    public async Task<JewelryItem> AddItemAsync(JewelryItem request)
    {
        var item = new JewelryItem
        {
            ItemName = request.ItemName,
            Price = request.Price,
            Category = request.Category,
            Quantity  = request.Quantity
        };

        var result = await _itemRepository.AddItemAsync(item);
        return result;
    }

    public async Task DeleteItemAsync(JewelryItem item)
    {
        await _itemRepository.DeleteItemAsync(item);
    }

    public async Task UpdateItemAsync(JewelryItem item, JewelryItem request)
    {
        item.ItemName = request.ItemName;
        item.Price = request.Price;
        item.Category = request.Category;
        item.Quantity = request.Quantity;
        item.UpdatedAt = DateTime.Now;

        await _itemRepository.UpdateItemAsync(item);
    }

    public async Task<IEnumerable<JewelryItem>> GeTAllItemsAsync()
    {
        return await _itemRepository.GeTAllItemsAsync();
    }

    public async Task<JewelryItem> GetItemByIdAsync(int id)
    {
        return await _itemRepository.GetItemByIdAsync(id);
    }
}
