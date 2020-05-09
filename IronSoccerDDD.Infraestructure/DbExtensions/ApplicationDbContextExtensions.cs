using IronSoccerDDD.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IronSoccerDDD.Infraestructure.DbExtensions
{
    public static class ApplicationDbContextExtensions
    {
        public static async Task EnsureSeedDataForContext(this ApplicationDbContext context)
        {
            await TryInitCountries(context);
            await TryInitTeam(context);
            await TryInitPlayers(context);
        }

        private async static Task TryInitPlayers(ApplicationDbContext context)
        {
            if (await context.Players.AnyAsync())
                return;

            var team = await context.Teams.FirstOrDefaultAsync();

            var francoNameResult = CompleteName.Create("Franco", "Armani");
            var quinterosNameResult = CompleteName.Create("Franco", "Armani");

            var francoEmail = Email.Create("farmani@farmani.com");
            var quinterosEmail = Email.Create("jfquintero@jfquintero.com");

            await context.Players.AddRangeAsync(new List<Player>()
            {
                new Player(francoNameResult.Value,  new DateTime(1986, 10, 16), francoEmail.Value , "1132123123",team),
                new Player(quinterosNameResult.Value,  new DateTime(1993, 1, 18), quinterosEmail.Value, "1132123123",team)
            });

            await context.SaveChangesAsync();
        }

        private async static Task TryInitTeam(ApplicationDbContext context)
        {
            if (await context.Teams.AnyAsync())
                return;

            await context.Teams.AddAsync(new Team("River", Country.Argentina));
            await context.SaveChangesAsync();
        }

        private async static Task TryInitCountries(ApplicationDbContext context)
        {
            if (await context.Countries.AnyAsync())
                return;

            await context.Countries.AddRangeAsync(Country.AllCountries);

            await context.SaveChangesAsync();
        }
    }
}
