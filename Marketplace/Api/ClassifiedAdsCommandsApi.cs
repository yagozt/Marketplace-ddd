using Marketplace.Contracts;
using Microsoft.AspNetCore.Mvc;
using Serilog;
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
        private readonly ClassifiedAdsApplicationService _applicationService;

        public ClassifiedAdsCommandsApi(ClassifiedAdsApplicationService applicationService)
        {
            _applicationService = applicationService;
        }

        [HttpPost]
        public async Task<IActionResult> Post(Contracts.ClassifiedAds.V1.Create request)
        {
            return await HandleRequest(request, _applicationService.Handle);
        }
        [HttpPut("name")]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.SetTitle request)
        {
            await _applicationService.Handle(request);
            return Ok();
        }
        [HttpPut("text")]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.UpdateText request)
        {
            await _applicationService.Handle(request);
            return Ok();
        }
        [HttpPut("price")]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.UpdatePrice request)
        {
            await _applicationService.Handle(request);
            return Ok();
        }
        [HttpPut("publish")]
        public async Task<IActionResult> Put(Contracts.ClassifiedAds.V1.RequestToPublish request)
        {
            await _applicationService.Handle(request);
            return Ok();
        }
        private async Task<IActionResult> HandleRequest<T>(T request, Func<T, Task> handler)
        {
            try
            {
                Log.Debug("Handling HTTP request of type {type}", typeof(T).Name);
                await handler(request);
                return Ok();
            }
            catch (System.Exception e)
            {
                Log.Error("Error handling the request", e);
                return new BadRequestObjectResult(new { error = e.Message, stackTrace = e.StackTrace });
            }
        }
    }
}
