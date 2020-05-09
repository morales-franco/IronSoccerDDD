using IronSoccerDDD.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronSoccerDDD.Infraestructure.EntityConfigurations
{
    public class TeamConfiguration : IEntityTypeConfiguration<Team>
    {
        public void Configure(EntityTypeBuilder<Team> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .Property(x => x.Name)
                .IsRequired(true)
                .HasMaxLength(200);

            builder
                .HasOne(x => x.Country)
                .WithMany(x=> x.Teams)
                .IsRequired(true);

            builder
                .HasMany(x => x.HomeMatches)
                .WithOne(x => x.TeamA);

            builder
                .HasMany(x => x.VisitorMatches)
                .WithOne(x => x.TeamB);

            builder
                .HasMany(x => x.Players)
                .WithOne(x => x.Team);
        }
    }
}
