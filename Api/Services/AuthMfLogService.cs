using ZionApi;
using ZionOrm;
using ModelVAS;
using Newtonsoft.Json.Linq;
using Api.Dtos;

namespace VasLog.Services
{
    public class AuthMfLogService : ZionCrud
    {
        public AuthMfLogService(string token_partner, long id_user, int success): base(request: "")
        {
            serviceName = "AuthMfLog";
            tableName = "auth_mf_log";
            dataModel = new AuthMfLogModel();
            dto = new AuthMfLogDto();
            json = LoadPartner(token_partner);
            dto.id_partner = GetJsonRecord(1, "id");
            dto.id_user = id_user;      
            dto.success = success;

            /*
             * Preste atenção em alterações neste serviço: 
             * Pois, gravar a tabela auth_mf_log, ela dispara uma cadeia de triggers no banco de dados para gerar estatisticas de requisição por partner
             *
             **/
            AuthMfLogModel AuthMfLogModel = new();
            dynamic model = new ZionModel(AuthMfLogModel, "auth_mf_log", "2");
            json = ZionDto.MapperModel(dto, ref model.RecordNew);
            json = model.Save("AuthMfLog");
        }
    }
}
