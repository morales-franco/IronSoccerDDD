using IronSoccerDDD.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace IronSoccerDDD.Infraestructure.EntityConfigurations
{
    internal class MatchConfiguration : IEntityTypeConfiguration<Match>
    {
        public void Configure(EntityTypeBuilder<Match> builder)
        {
            builder
                .HasKey(x => x.Id);

            builder
                .HasOne(x => x.BestPlayer)
                .WithMany()
                .IsRequired(false);

            builder
               .HasOne(x => x.Winner)
               .WithMany()
               .IsRequired(false);

            builder
                .HasOne(p => p.TeamA)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

            builder
                .HasOne(p => p.TeamB)
                .WithMany()
                .IsRequired()
                .OnDelete(DeleteBehavior.NoAction);

        }
    }
}
