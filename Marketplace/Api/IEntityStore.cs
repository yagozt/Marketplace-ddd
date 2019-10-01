using System.Threading.Tasks;

namespace Marketplace.Api
{
    public interface IEntityStore{
        Task<T> Load<T>(string id);
        Task Save<T>(T entity);
    }
}