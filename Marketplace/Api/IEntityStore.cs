using System.Threading.Tasks;
using Marketplace.Framework;

namespace Marketplace.Api
{
    public interface IEntityStore{
        Task<T> Load<T>(string id) where T : Entity;
        Task Save<T>(T entity) where T : Entity;
        Task<bool> Exists<T>(string entityId);

    }
}