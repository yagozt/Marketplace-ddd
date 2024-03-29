using System;
using System.Threading.Tasks;
using Marketplace.Domain;
using Marketplace.Framework;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IClassifiedAdRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private ICurrencyLookup _currencyLookup;

        public ClassifiedAdsApplicationService(IClassifiedAdRepository repository, IUnitOfWork unitOfWork, ICurrencyLookup currencyLookup)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _currencyLookup = currencyLookup;
        }

        public Task Handle(object command) =>
            command switch
            {
                V1.Create cmd => HandleCreate(cmd),
                V1.SetTitle cmd => HandleUpdate(cmd.Id, c => c.SetTitle(ClassifiedAdTitle.FromString(cmd.Title))),
                V1.UpdateText cmd => HandleUpdate(cmd.Id, c => c.UpdateText(ClassifiedAdText.FromString(cmd.Text))),
                V1.UpdatePrice cmd => HandleUpdate(cmd.Id, c => c.UpdatePrice(Price.FromDecimal(cmd.Price, cmd.Currency, _currencyLookup))),
                V1.RequestToPublish cmd => HandleUpdate(cmd.Id, c => c.RequestToPublish()),
                _ => Task.CompletedTask
            };
        private async Task HandleCreate(V1.Create cmd)
        {
            if (await _repository.Exists(new ClassifiedAdId(cmd.Id)))
                throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");

            var classifiedAd = new ClassifiedAd(new ClassifiedAdId(cmd.Id),
                                                    new UserId(cmd.OwnerId));

            await _repository.Add(classifiedAd);
        }
        private async Task HandleUpdate(Guid classifiedAdId, Action<ClassifiedAd> operation)
        {
            var classifiedAd = await _repository.Load(new ClassifiedAdId(classifiedAdId));
            if (classifiedAd == null)
                throw new InvalidOperationException($"{classifiedAdId} not found");

            operation(classifiedAd);

            await _repository.Add(classifiedAd);
        }
    }
}