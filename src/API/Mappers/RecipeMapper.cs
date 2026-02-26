using System.Collections.Generic;
using System.Linq;
using API.DTOs;
using Domain.Entities;

namespace API.Mappers
{
    public static class RecipeMapper
    {
        public static RecipeDto ToDto(this Recipe recipe)
        {
            return new RecipeDto
            {
                Id = recipe.Id,
                Title = recipe.Title,
                Description = recipe.Description,
                Categories = recipe.Categories?.Select(c => c.Name).ToList(),
                Ingredients = recipe.Ingredients.ToList() ?? [],
            };
        }

        public static IEnumerable<RecipeDto> ToDtos(this IEnumerable<Recipe> recipes)
        {
            return recipes.Select(r => r.ToDto());
        }

        public static Recipe ToEntity(this RecipeDto recipeDto)
        {
            return new Recipe
            {
                Id = recipeDto.Id,
                Title = recipeDto.Title,
                Description = recipeDto.Description,
                Ingredients = recipeDto.Ingredients?.ToList() ?? [],
            };
        }
    }
}
