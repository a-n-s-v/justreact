using ESI.Data.CosmosDb;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ESI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly ICosmosContext cosmosContext;

        public TestController(ICosmosContext cosmosContext)
        {
            this.cosmosContext = cosmosContext;
        }

        [HttpGet]
        public async Task<IActionResult> Test()
        {
            await cosmosContext.Test();
            return NoContent();
        }
    }
}
