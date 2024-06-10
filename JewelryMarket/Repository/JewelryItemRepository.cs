using JewelryMarket.Data;
using JewelryMarket.Entities;
using JewelryMarket.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace JewelryMarket.Repository;

public class JewelryItemRepository : IJewelryItemRepository
{
    private readonly AppDbContext _context;
    public JewelryItemRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task<JewelryItem> AddItemAsync(JewelryItem item)
    {
        await _context.JewelryItems.AddAsync(item);
        await _context.SaveChangesAsync();
        
        return item;
    }

    public async Task DeleteItemAsync(JewelryItem item)
    {
        _context.JewelryItems.Remove(item);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateItemAsync(JewelryItem item)
    {
        _context.JewelryItems.Update(item);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<JewelryItem>> GeTAllItemsAsync()
    {
        return await _context.JewelryItems.ToListAsync();
    }

    public async Task<JewelryItem> GetItemByIdAsync(int id)
    {
        return await _context.JewelryItems.FindAsync(id);
    }        
}
