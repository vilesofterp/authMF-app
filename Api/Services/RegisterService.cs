
using Newtonsoft.Json.Linq;
using ZionApi;
using ZionOrm;
using Api.Dtos;
using ZionHelper;
using Api.Models;

namespace Api.Services
{
    public class RegisterService : ZionCrud
    {
        public RegisterService(object request) : base(request)
        {
            serviceName = "Register";
            tableName = "user";
            dataModel = new UserModel();
            dto = new RegisterDto();
        }
        public JObject Register()
        {
            ZionDto.UnMappedFields = "token_auth_private";
            json = ZionDto.MapperDto(ref dto, base.requestData);

            if (GetJson("status") != "success")
            {
                return ZionResponse.Fail(api_error: 23, body: "Dto Mapper", statusCode: 400, "auto", messageLog: ZionDto.GetResult());
            }

            string filter = "id = " + SqlPar(dto.Id.ToString()) + " and email = " + SqlPar(dto.Email.ToLower()) + " and active = 1 and deleted = 0";
            json = Get(filter, "name");

            if (GetJson("status") != "xsuccess")
            {
                return json;
            }

            double countRows = ZionConv.ToDouble(GetJsonSummary("records_query"));

            if (countRows != 1)  
            {
                return ZionResponse.Fail(api_error: 53, body: "", statusCode: 400, "auto", messageLog: "User not found with id and email received: " + LastSqlSentence);
            }

            // App registration
            ZionTotp totp = new ZionTotp();
            dto.Token_auth_private = totp.NewPrivateKey();
            
            JObject response = new JObject();
            response.Add("token_auth_private", ZionSecurity.Mask(dto.Token_auth_private, offset: 24));
            return ZionResponse.Success(response);
        }
    }
}
