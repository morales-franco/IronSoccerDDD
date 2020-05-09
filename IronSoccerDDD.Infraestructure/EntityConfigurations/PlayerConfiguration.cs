using IronSoccerDDD.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace IronSoccerDDD.Infraestructure.EntityConfigurations
{
    internal class PlayerConfiguration : IEntityTypeConfiguration<Player>
    {
        public void Configure(EntityTypeBuilder<Player> builder)
        {
            builder
                .HasKey(x => x.Id);

            /*
             * TODO: Implementing VALUE-OBJECTS AS OWN ENTITY
             * Watch out: Bug with IsRequired() statement we need to do a manual migration to set fields as Required - CompleteNameRequiredPlayer (our migration)
             * E.F by default creates queries like CompleteName is a child/Own entity (othe table), for this reason, the queries are more complex as need it.
             */

            builder
                .OwnsOne(x => x.CompleteName, x =>
                {
                    x.Property(y => y.FirstName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName(nameof(CompleteName.FirstName));

                    x.Property(y => y.LastName)
                    .IsRequired()
                    .HasMaxLength(150)
                    .HasColumnName(nameof(CompleteName.LastName));
                });

            /*
             * TODO: Implementing VALUE-OBJECT using VALUE CONVERTIONS
             * In this case, we tell EF how It should create new entities from string to Email and viceversa.
             * Email has a single property, for this cases VALUE CONVERTIONS are a great alternative.
             */

            builder
               .Property(x => x.Email)
               .IsRequired(true)
               .HasMaxLength(300)
               .HasConversion(x => x.Value, p => Email.Create(p).Value);

            builder
               .Property(x => x.Phone)
               .IsRequired(true)
               .HasMaxLength(50);


            builder
                .HasOne(x => x.Team)
                .WithMany(x => x.Players)
                .IsRequired(true);
        }
    }
}
