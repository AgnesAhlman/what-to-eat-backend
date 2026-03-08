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
                Ingredients = recipe
                    .Ingredients?.Select(i => new IngredientDto
                    {
                        Text = i.Text,
                        Unit = i.Unit,
                        Amount = i.Amount,
                    })
                    .ToList(),
            };
        }

        public static IEnumerable<RecipeDto> ToDtos(this IEnumerable<Recipe> recipes)
        {
            return recipes.Select(r => r.ToDto());
        }

        public static Recipe ToEntity(this RecipeDto recipeDto)
        {
            var recipe = new Recipe
            {
                Id = recipeDto.Id,
                Title = recipeDto.Title,
                Description = recipeDto.Description,
            };

            if (recipeDto.Ingredients != null)
            {
                foreach (var i in recipeDto.Ingredients)
                {
                    recipe.Ingredients.Add(new Ingredient
                    {
                        Text = i.Text,
                        Unit = i.Unit,
                        Amount = i.Amount,
                    });
                }
            }

            return recipe;
        }
    }
}
