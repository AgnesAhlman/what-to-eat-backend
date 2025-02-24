using System.ComponentModel.DataAnnotations;

namespace WhatToEat.Entities
{
    public class Recipe
    {
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        public string Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
