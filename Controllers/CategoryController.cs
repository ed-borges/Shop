
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

namespace Shop.Data;

[Route("categories")]
[ApiController]
public class CategoryController(DataContext context) : ControllerBase
{

    [HttpGet, Route("")]
    public async Task<ActionResult<List<Category>>> Get()
    {
        return Ok(await context.Categories.AsNoTracking().ToListAsync());
    }

    [HttpGet, Route("{id:int}")]
    public async Task<ActionResult<Category>> GetById(int id)
    {
        var model = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

        if (model == null)
            return NotFound(new { message = "Categpria não encontrada" });

        return Ok(model);
    }

    [HttpPost, Route("")]
    public async Task<ActionResult<Category>> Post([FromBody] Category model)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            context.Categories.Add(model);
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel criar a categpria" });
        }

    }

    [HttpPut, Route("{id:int}")]
    public async Task<ActionResult<Category>> Put(int id, [FromBody] Category model)
    {
        if (model.Id != id)
            return NotFound(new { message = "Categpria não encontrada" });

        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        try
        {
            context.Entry<Category>(model).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(model);
        }
        catch (DbUpdateConcurrencyException)
        {
            return BadRequest(new { message = "Não foi possivel actualizar a categpria" });
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel actualizar a categpria" });
        }
    }

    [HttpDelete, Route("{id:int}")]
    public async Task<ActionResult> Delete(int id)
    {

        var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

        if (category is null)
            return NotFound(new { message = "Categpria não encontrada" });


        try
        {
            context.Remove(category);
            await context.SaveChangesAsync();
            return NoContent();
        }
        catch (Exception)
        {
            return BadRequest(new { message = "Não foi possivel remover a categpria" });
        }
    }
}