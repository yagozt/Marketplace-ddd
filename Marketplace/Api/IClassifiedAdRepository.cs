using System.Threading.Tasks;
using Marketplace.Domain;

namespace Marketplace.Api
{
    public interface IClassifiedAdRepository
    {
        Task<bool> Exists(string v);
        Task Save(ClassifiedAd classifiedAd);
        Task<ClassifiedAd> Load(string v);
    }
}