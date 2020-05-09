using System.Threading.Tasks;
using IronSoccerDDD.Core.Entities;
using IronSoccerDDD.Core.IRepositories;

namespace IronSoccerDDD.Infraestructure.Repositories
{
    public class TeamRepository : EfRepository<Team, int>, ITeamRepository
    {
        public TeamRepository(ApplicationDbContext context) 
            : base(context)
        {
        }

        public async override Task<Team> GetByIdAsync(int id)
        {
            var entity = await base.GetByIdAsync(id);

            if (entity == null)
                return null;

            /*
             * TODO: LAZY LOAD & FIELDS & PROPERTY COLLECTIONS
             * Specify accessing to COLLECTION through FIELDS and NOT using NAVIGATION PROPERTIES.
             * In our case, we manage collection internally through PRIVATE FIELDS.
             * When we access to PRIVATE FIELD as _myCollection, this propert will not have any data.
             * Because by default LAZY LOAD inflate NAVIGATION PROPERTY and NOT FIELDS.
             * Players --> WILL BE LOADED
             * _players --> WILL BE EMPTY.
             * 
             * --> After call this.Players.Any(x=> ...) or consult Players, _players will be populated.
             * But it is not the idea, in the internal class: we must use the private property _players and
             * not the PROPERTY Players.
             * 
             * We have two options: 
             * 1. EAGER LOADING ==> Specify includes in the query ==> that behaviour load the private field too.
             * 2. EXPLICIT LOADING ==> Specify explicit loading entities ==> that behaviour load the private field too ==> Recommended
             */

            _context.Entry(entity).Collection(x => x.Players).Load();
            _context.Entry(entity).Collection(x => x.HomeMatches).Load();
            _context.Entry(entity).Collection(x => x.VisitorMatches).Load();

            return entity;
        }
    }
}
