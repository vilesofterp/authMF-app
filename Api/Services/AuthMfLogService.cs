using ZionApi;
using ZionOrm;
using ModelVAS;
using Api.Dtos;
using System.Data;

namespace Api.Services
{
    public class AuthMfLogService : ZionCrud
    {
        public AuthMfLogService(string token_partner, long id_user, int success) : base(request: new CustomRequestForm())
        {
            serviceName = "AuthMfLog";
            tableName = "auth_mf_log";
            dataModel = new AuthMfLogModel();
            dto = new AuthMfLogDto();
            DataTable tb = LoadPartner(token_partner);
            dto.id_partner = (long)tb.Rows[0]["ID"];
            dto.id_user = id_user;      
            dto.success = success;
            ZionModel model = new ZionModel(dataModel, "auth_mf_log", "2");
            json = ZionDto.MapperModel(in dto, ref model.RecordNew);
            json = model.Save("AuthMfLogService");
        }
    }
}
