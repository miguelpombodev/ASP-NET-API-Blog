using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    [Route("v1/categories")]
    public class CategoryController : ControllerBase
    {
        [HttpGet("")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context
        )
        {
            try
            {
                List<Category> categories = await context.Categories.ToListAsync();

                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (System.Exception)
            {
                return StatusCode(500, new ResultViewModel<List<Category>>(error: "Internal Server Error"));
            }
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id
        )
        {
            try
            {
                Category category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null) return NotFound(new ResultViewModel<Category>(error: "Content not found"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch (System.Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("Internal Server Error"));
            }

        }


        [HttpPost("")]
        public async Task<IActionResult> CreateAsync(
            [FromServices] BlogDataContext context,
            [FromBody] EditorCategoryViewModel model
        )
        {
            if (!ModelState.IsValid) return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var category = new Category
                {
                    Id = 0,
                    Name = model.Name,
                    Slug = model.Slug.ToLower()
                };

                await context.Categories.AddAsync(category);
                await context.SaveChangesAsync();

                return Created($"v1/categories/{category.Id}", new ResultViewModel<Category>(category));
            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "It was not possible to register the category in database"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "Internal Server Error"));
            }
        }

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateAsync(
            [FromServices] BlogDataContext context,
            [FromBody] EditorCategoryViewModel model,
            [FromRoute] int id
        )
        {
            try
            {
                Category category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null) return NotFound(new ResultViewModel<Category>(error: "Content not found"));

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));

            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "It was not possible to update the category in database"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "Internal Server Error"));
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices] BlogDataContext context,
            [FromRoute] int id
        )
        {
            try
            {
                Category category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

                if (category == null) return NotFound(new ResultViewModel<Category>(error: "Content not found"));

                context.Categories.Remove(category);

                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));

            }
            catch (DbUpdateException e)
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "It was not possible to delete the category in database"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>(error: "Internal Server Error"));
            }
        }
    }
}