using Api.Dtos;
using ZionOrm;
using ZionApi;
using ZionHelper;
using ModelVAS;
namespace Api.Services
{
    public class UserService : ZionCrud
    {
        public UserService(object request) : base(request: request)
        {
            serviceName = "GetPartners";
            tableName = "user";
            dataModel = new UserModel();
            dto = new GetPartnersDto();
        }

        public dynamic LoadUser(string filter)
        {
            json = Get(filter);

            if (GetJson("status") != "success" || ZionConv.ToInt(GetJsonSummary("records_json")) == 0)
            {
                throw new ZionException(59, "auto", "UserService.LoadUser()", "Unable to authenticate the API with the data received in Dto: " + LastSqlSentence);
            }

            return json;
        }
    }
}

