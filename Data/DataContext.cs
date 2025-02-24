using Microsoft.EntityFrameworkCore;
using WhatToEat.Entities;

namespace WhatToEat.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Recipe> Recipes { get; set; }
    }
}