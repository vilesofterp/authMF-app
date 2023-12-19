using Microsoft.AspNetCore.Mvc;
using Api.Services;
using Microsoft.AspNetCore.Mvc.Formatters;

namespace Api
{
    [ApiController]
    [Route("api/[controller]")]
    public class ValidateCodeController : ControllerBase
    {
        [HttpPost()]
        public string ValidateTotp()
        {
            ValidateCodeService service = new ValidateCodeService(request: Request.Form);
            return service.ValidateCode().ToString();
        }
    }
}
