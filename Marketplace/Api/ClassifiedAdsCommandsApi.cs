using Marketplace.Contracts;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using static Marketplace.Contracts.ClassifiedAds;

namespace Marketplace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        private readonly Func<IHandleCommand<V1.Create>> _createAdCommandHandlerFactory;

        public ClassifiedAdsCommandsApi(Func<IHandleCommand<V1.Create>> createAdCommandHandlerFactory)
        {
            _createAdCommandHandlerFactory = createAdCommandHandlerFactory;
        }

        [HttpPost]
        public Task Post(Contracts.ClassifiedAds.V1.Create request)
        {
            return _createAdCommandHandlerFactory().Handle(request);
        }
    }
}
