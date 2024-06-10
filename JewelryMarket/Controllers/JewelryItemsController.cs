using JewelryMarket.Entities;
using JewelryMarket.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace JewelryMarket.Controllers;

[Route("api/[controller]")]
[ApiController]
public class JewelryItemsController : ControllerBase
{
    private readonly IJewelryItemService _jewelryItemService;

    public JewelryItemsController(IJewelryItemService jewelryItemService)
    {
        _jewelryItemService = jewelryItemService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<JewelryItem>>> GetAllItems([FromQuery] int? category)
    {
        var items = await _jewelryItemService.GeTAllItemsAsync();
        if (category.HasValue)
        {
            items = items.Where(i => (int)i.Category == category.Value);
        }
        return Ok(items);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<JewelryItem>> GetItemById(int id)
    {
        var item = await _jewelryItemService.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }
        return Ok(item);
    }

    [HttpPost("additem"), Authorize(Roles = "Admin")]
    public async Task<ActionResult<JewelryItem>> AddItem(JewelryItem request)
    {
        var result = await _jewelryItemService.AddItemAsync(request);

        return Ok(result);
    }

    [HttpPut("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateItem(int id, JewelryItem request)
    {
        // check if item exists
        var item = await _jewelryItemService.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        await _jewelryItemService.UpdateItemAsync(item, request);

        return NoContent();
    }

    [HttpDelete("{id}"), Authorize(Roles = "Admin")]
    public async Task<IActionResult> DeleteItem(int id)
    {
        var item = await _jewelryItemService.GetItemByIdAsync(id);
        if (item == null)
        {
            return NotFound();
        }

        await _jewelryItemService.DeleteItemAsync(item);

        return NoContent();
    }
}

