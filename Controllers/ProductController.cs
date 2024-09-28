
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;

namespace Shop.Controllers;

[ApiController]
[Route("produts")]
public class ProductController(DataContext context) : ControllerBase
{

    [HttpGet, Route("")]
    public async Task<ActionResult<List<Product>>> Get()
    {
        var products = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .ToListAsync();
        return Ok(products);
    }

    [HttpGet, Route("{id:int}")]
    public async Task<ActionResult<Product>> GetById(int id)
    {
        var model = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (model == null)
            return NotFound(new { message = "Produto não encontrada" });

        return Ok(model);
    }

    [HttpGet, Route("categories/{id:int}")]
    public async Task<ActionResult<List<Product>>> GetByCategory(int id)
    {
        var products = await context
            .Products
            .Include(x => x.Category)
            .AsNoTracking()
            .Where(x => x.CategoriaId == id)
            .ToListAsync();


        return Ok(products);
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<Category>> Post([FromBody] Product model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            context.Products.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel criar o produto" });
        }

    }

    [HttpPut, Route("{id:int}")]
    public async Task<ActionResult<Product>> Put(int id, [FromBody] Product model)
    {
        if (model.Id != id)
            return NotFound(new { message = "Produto não encontrada" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            context.Entry<Product>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Não foi possivel actualizar o produto" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel actualizar o produto" });
        }
    }

    [HttpDelete, Route("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {

        var product = await context
            .Products
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);

        if (product is null)
            return NotFound(new { message = "Produto não encontrada" });


        try
        {
            context.Remove(product);
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel remover o produto" });
        }
    }

}