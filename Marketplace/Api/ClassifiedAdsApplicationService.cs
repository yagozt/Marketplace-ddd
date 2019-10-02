using System;
using System.Threading.Tasks;
using Marketplace.Domain;
using Marketplace.Framework;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    public class ClassifiedAdsApplicationService : IApplicationService
    {
        private readonly IEntityStore _store;
        private ICurrencyLookup _currencyLookup;

        public ClassifiedAdsApplicationService(IEntityStore store, ICurrencyLookup currencyLookup)
        {
            _store = store;
            _currencyLookup = currencyLookup;
        }

        public async Task Handle(object command)
        {
            ClassifiedAd classifiedAd;
            switch(command){
                case V1.Create cmd:
                    if(await _store.Exists<ClassifiedAd>(cmd.Id.ToString()))
                        throw new InvalidOperationException($"Entity with id {cmd.Id} already exists");
                    
                    classifiedAd = new ClassifiedAd(new ClassifiedAdId(cmd.Id),
                                                    new UserId(cmd.OwnerId));

                    await _store.Save(classifiedAd);  
                    break;
                case V1.SetTitle cmd:
                    classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
                    if(classifiedAd == null)
                        throw new InvalidOperationException($"{cmd.Id} not found");
                    
                    classifiedAd.SetTitle(ClassifiedAdTitle.FromString(cmd.Title));
                    await _store.Save(classifiedAd);
                    break;
                case V1.UpdateText cmd:
                    classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
                    if(classifiedAd == null)
                        throw new InvalidOperationException($"{cmd.Id} not found");
                    
                    classifiedAd.UpdateText(ClassifiedAdText.FromString(cmd.Text));
                    await _store.Save(classifiedAd);
                    break;
                case V1.UpdatePrice cmd:
                    classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
                    if(classifiedAd == null)
                        throw new InvalidOperationException($"{cmd.Id} not found");
                    
                    classifiedAd.UpdatePrice(Price.FromDecimal(cmd.Price,cmd.Currency,_currencyLookup));
                    await _store.Save(classifiedAd);
                    break;
                case V1.RequestToPublish cmd:
                    classifiedAd = await _store.Load<ClassifiedAd>(cmd.Id.ToString());
                    if(classifiedAd == null)
                        throw new InvalidOperationException($"{cmd.Id} not found");
                    
                    classifiedAd.RequestToPublish();
                    await _store.Save(classifiedAd);
                    break;
                default: throw new InvalidOperationException($"Command type {command.GetType().FullName} is unknown");
            }
        }    
    }
}