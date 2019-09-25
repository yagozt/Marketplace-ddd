using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Marketplace.Api
{
    [Route("/ad")]
    public class ClassifiedAdsCommandsApi : Controller
    {
        [HttpPost]
        public async Task<IActionResult> Post(Contracts.ClassifiedAds.V1.Create request)
        {
            // handle
            return Ok();
        }
    }
}
