using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ZUSContributionImporter.Contributions;
using ZUSContributionImporter.Models;

namespace ZUSContributionImporter.Controllers
{
    [Route("api/[controller]")]
    public class ContributionController : Controller
    {
        private readonly IConfigurationRoot _config;

        public ContributionController(IConfigurationRoot config)
        {
            _config = config;
        }

        [HttpGet("{validityDate:datetime}")]
        public ActionResult Get(DateTime validityDate)
        {
            var queryHandler = new ContributionQueryHandler(_config);
            var task = queryHandler.HandleAsync(new ContributionQuery { ValidityDate = validityDate });

            var contribution = task.Result;

            return Json(new ApiResponse<Contribution> { Success = true, Model = contribution });
        }
    }
}