using Microsoft.AspNetCore.Mvc;
using ZionOrm;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class TesteController : ControllerBase
    {
        [HttpGet()]
        public string Teste()
        {
            IOrm orm = new Orm();
            orm.Delete("company", "id = 1");
            return orm.GetRowsAffected().ToString();
        }
    }
}
