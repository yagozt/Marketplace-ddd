using System.Threading.Tasks;

namespace Marketplace.Domain
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(ClassifiedAdId id);
        Task Add(ClassifiedAd entity);
        Task<ClassifiedAd> Load(ClassifiedAdId id);
    }
}