using ZionApi;
using ZionOrm;
using VasLog.Dtos;
using ModelVAS;
using Newtonsoft.Json.Linq;
using Api.Dtos;

namespace VasLog.Services
{
    public class AuthMfLogService
    {
        public AuthMfLogService(long id_partner, long id_user, int success)
        {
            AuthMfLogDto dto = new();
            dto.id_partner = id_partner;
            dto.id_user = id_user;      
            dto.success = success;

            AuthMfLogModel authMfLogModel = new();
            dynamic model = new ZionModel(authMfLogModel, "auth_mf_log", "2");
            JObject json = ZionDto.MapperModel(dto, ref model.RecordNew);
            json = model.Save("AuthMfLog");

        }
    }
}
