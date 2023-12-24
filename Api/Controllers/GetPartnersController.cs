using Microsoft.AspNetCore.Mvc;
using Api.Services;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class GetPartnersController : ControllerBase
    {
        [HttpPost()]
        public string GetPartners()
        {
            GetPartnersService service = new GetPartnersService(request: Request.Form);
            return service.GetPartners().ToString();
        }
    }
}
