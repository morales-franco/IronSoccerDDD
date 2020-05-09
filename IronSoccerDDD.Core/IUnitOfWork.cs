using System.Threading.Tasks;

namespace IronSoccerDDD.Core
{
    public interface IUnitOfWork
    {
        Task<bool> Commit();
    }
}
