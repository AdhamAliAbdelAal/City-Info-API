using CityInfoApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CityInfoApi.DbContexts;

public class CityInfoDbContext : DbContext
{
    public CityInfoDbContext(DbContextOptions<CityInfoDbContext> options) :base(options)
    {
        
    }

    public DbSet<CityDbModel> Cities { get; set; } = null!;
    public DbSet<PointOfInterestDbModel> PointsOfInterest { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CityDbModel>()
            .HasData(
                new CityDbModel()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park."
                },
                new CityDbModel()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished."
                },
                new CityDbModel()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower."
                });
        
        modelBuilder.Entity<PointOfInterestDbModel>()
            .HasData(
                new PointOfInterestDbModel
                {
                    Id = 1,
                    CityId = 1,
                    Name = "Central Park",
                    Description = "The most visited urban park in the United States."
                },
                new PointOfInterestDbModel
                {
                    Id = 2,
                    CityId = 1,
                    Name = "Empire State Building",
                    Description = "A 102-story skyscraper located in Midtown Manhattan."
                },
                new PointOfInterestDbModel
                {
                    Id = 3,
                    CityId = 2,
                    Name = "Cathedral",
                    Description = "A Gothic style cathedral, conceived by architects Jan and Pieter Appelmans."
                },
                new PointOfInterestDbModel()
                {
                    Id = 4,
                    CityId = 2,
                    Name = "Antwerp Central Station",
                    Description = "The the finest example of railway architecture in Belgium."
                },
                new PointOfInterestDbModel
                {
                    Id = 5,
                    CityId = 3,
                    Name = "Eiffel Tower",
                    Description = "A wrought iron lattice tower on the Champ de Mars, named after engineer Gustave Eiffel."
                },
                new PointOfInterestDbModel
                {
                    Id = 6,
                    CityId = 3,
                    Name= "The Louvre",
                    Description = "The world's largest museum."
                }
            );
        base.OnModelCreating(modelBuilder);
    }

}