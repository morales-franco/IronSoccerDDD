using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;

namespace IronSoccerDDD.Infraestructure.Repositories
{
    public class CountryRepository : EfRepository<Country, int>, ICountryRepository
    {
        public CountryRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
