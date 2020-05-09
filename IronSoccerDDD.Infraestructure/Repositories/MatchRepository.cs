using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;

namespace IronSoccerDDD.Infraestructure.Repositories
{
    public class MatchRepository : EfRepository<Match, int>, IMatchRepository
    {
        public MatchRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
