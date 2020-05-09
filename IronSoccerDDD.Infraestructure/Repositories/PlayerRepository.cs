using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;

namespace IronSoccerDDD.Infraestructure.Repositories
{
    public class PlayerRepository : EfRepository<Player, int>, IPlayerRepository
    {
        public PlayerRepository(ApplicationDbContext context) 
            : base(context)
        {
        }
    }
}
