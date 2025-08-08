using API.DTOs;
using API.Mappers;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RecipeController(ApplicationDbContext dbContext) : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext = dbContext;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RecipeDto>>> GetRecipes()
        {
            var recipes = await _dbContext.Recipes.Include(r => r.Categories).ToListAsync();
            return Ok(recipes.ToDtos());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RecipeDto>> GetRecipe(int id)
        {
            var recipe = await _dbContext
                .Recipes.Include(r => r.Categories)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (recipe == null)
            {
                return NotFound();
            }

            return Ok(recipe.ToDto());
        }

        [HttpPost]
        public async Task<ActionResult<RecipeDto>> CreateRecipe(RecipeDto recipeDto)
        {
            var recipe = recipeDto.ToEntity();

            // Handle categories if provided
            if (recipeDto.Categories != null && recipeDto.Categories.Count != 0)
            {
                foreach (var categoryName in recipeDto.Categories)
                {
                    var category = await _dbContext.Categories.FirstOrDefaultAsync(c =>
                        c.Name == categoryName
                    );

                    if (category == null)
                    {
                        category = new Category { Name = categoryName };
                        _dbContext.Categories.Add(category);
                    }

                    recipe.Categories.Add(category);
                }
            }

            _dbContext.Recipes.Add(recipe);
            await _dbContext.SaveChangesAsync();

            // Reload the recipe with categories to ensure we have all the data
            var createdRecipe = await _dbContext
                .Recipes.Include(r => r.Categories)
                .FirstOrDefaultAsync(r => r.Id == recipe.Id);

            return CreatedAtAction(
                nameof(GetRecipes),
                new { id = recipe.Id },
                createdRecipe != null ? createdRecipe.ToDto() : recipe.ToDto()
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRecipe(int id)
        {
            var recipe = await _dbContext.Recipes.FindAsync(id);

            if (recipe == null)
            {
                return NotFound();
            }

            _dbContext.Recipes.Remove(recipe);
            await _dbContext.SaveChangesAsync();

            return NoContent();
        }
    }
}
