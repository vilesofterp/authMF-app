using Microsoft.AspNetCore.Mvc;
using Api.Services;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegisterController : ControllerBase
    {
        [HttpPost()]
        public string Register()
        {
            RegisterService service = new RegisterService(request: Request.Form);
            return service.Register().ToString();
        }
    }
}
