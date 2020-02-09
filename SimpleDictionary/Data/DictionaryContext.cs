using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SimpleDictionary.Models.DataModels;

namespace SimpleDictionary.Data
{
    public class DictionaryContext : IdentityDbContext
    {
        public DictionaryContext(DbContextOptions<DictionaryContext> options) : base(options)
        { 
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {

            builder.Entity<User>()
                .HasMany(d => d.Definitions)
                .WithOne(u => u.Author)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Entity<Hashtag>()
                .HasOne(d => d.Definition)
                .WithMany(h => h.Hashtags)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            base.OnModelCreating(builder);
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Definition> Definitions { get; set; }
        public DbSet<LikedDefinition> LikedDefinition { get; set; }
        public DbSet<DislikedDefinition> DislikedDefinition { get; set; }
    }
}
