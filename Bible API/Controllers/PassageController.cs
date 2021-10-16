using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Bible_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PassageController : ControllerBase
    {
        /// <summary>
        /// Retrieve a 
        /// </summary>
        /// <remarks>
        /// </remarks>
        /// <param name="passage"></param>
        /// <param name="version"></param>
        /// <param name="language"></param>
        public PassageController(string passage, string version, string language)
        {
        }

    }
}