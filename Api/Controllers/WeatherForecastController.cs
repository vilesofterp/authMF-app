using Microsoft.AspNetCore.Mvc;


namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class LogServerController : ControllerBase
    {
        [HttpPost()]
        public dynamic LogServer()
        {
            return Ok("teste");
        }
    }
}
