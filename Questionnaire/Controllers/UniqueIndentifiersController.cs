using Microsoft.AspNetCore.Mvc;
using System;

namespace Questionnaire.Controllers
{
    [Produces("application/json")]
    [Route("api/UniqueIndentifiers")]
    public class UniqueIndentifiersController : Controller
    {
        [HttpGet, Route("NewGuid")]
        public dynamic NewGuid()
        {
            return new
            {
                UniqueIndentifier = Guid.NewGuid().ToString("D")
            };
        }
    }
}