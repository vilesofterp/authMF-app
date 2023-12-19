using ZionApi;
using ZionOrm;
using ModelVAS;
using Newtonsoft.Json.Linq;
using Api.Dtos;

namespace VasLog.Services
{
    public class AuthMfStatService
    {
        public AuthMfStatService(long id_partner, long id_user, int success)
        {
            AuthMfStatDto dto = new();
            dto.id_partner = id_partner;
            dto.id_user = id_user;      
            dto.success = success;

            AuthMfStatModel AuthMfStatModel = new();
            dynamic model = new ZionModel(AuthMfStatModel, "auth_mf_log", "2");
            JObject json = ZionDto.MapperModel(dto, ref model.RecordNew);
            json = model.Save("AuthMfStat");
        }
    }
}
