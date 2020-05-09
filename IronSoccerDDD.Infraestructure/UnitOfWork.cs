using IronSoccerDDD.Core;
using System;
using System.Threading.Tasks;

namespace IronSoccerDDD.Infraestructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private ApplicationDbContext _context;

        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<bool> Commit()
        {
            return (await _context.SaveChangesAsync()) >= 0;
        }
    }
}
