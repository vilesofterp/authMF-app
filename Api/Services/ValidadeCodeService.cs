using Api.Dtos;
using ZionOrm;
using ModelVAS;
using ZionHelper;
using VasLog.Services;
using ZionApi;

namespace Api.Services
{
    public class ValidateCodeService : ZionCrud
    {
        public ValidateCodeService(object request) : base(request)
        {
            serviceName = "ValidateCode";
            tableName = "user";
            dataModel = new UserModel();
            dto = new ValidateCodeDto();
        }

        public dynamic ValidateCode()
        {
            string messageLog = "";

            json = ZionDto.MapperDto(ref dto, requestData);

            if (GetJson("status") != "success")
            {
                messageLog = ZionDto.GetResult();
                return ZionResponse.Fail(23, "", 400, "auto", messageLog);
            }

            sqlQuery = "select * from auth_mf where id_user = " + SqlPar(dto.Id_user.ToString()) + " and id_partner = (select id from partner where token = " + SqlPar(dto.Token_partner) + ") and active = 1";
            json = Get();

            if (GetJson("status") != "success" || ZionConv.ToInt(GetJsonSummary("records_json")) == 0)
            {
                throw new ZionException(59, "auto", "ValidateCode()", "Partner not found - Unable to authenticate the API with the data received in Dto: " + LastSqlSentence);
            }

            string id_partner = GetJsonRecord(1, "id_partner");
            string id_user = GetJsonRecord(1, "id_user");
            string public_key = GetJsonRecord(1, "public_key");
            string private_key = GetJsonRecord(1, "private_key");
            string hashKey = id_user + id_partner + public_key;
            ZionEncrypter hash = new ZionEncrypter();

            if (!hash.CompareHash(private_key, hashKey))
            {
                messageLog = $"user: {id_user} partner: {id_partner} - Suspected violation of the public key {public_key} in the generation of totp. Private key result incorrect";
                ZionAlert.Save(messageLog);
                return ZionResponse.Fail(63, "", 400, "auto", messageLog);
            }
                                      
            // Code Validation
            ZionTotp totp = new ZionTotp();
            json = totp.VerifyTotp(dto.Code, ZionSecurity.UnMask(public_key, 64));
            int success = GetJson("status") == "success" ? 1 : 0;
            int userLogCode = success == 1 ? 23 : 24;

            // user log
            UserLogService userlogService = new UserLogService(
                id_user: dto.Id_user,
                code: userLogCode,
                operation: "",
                latitude: dto.Latitude,
                longitude: dto.Longitude,
                ip: dto.Ip
            );

            // Auth_mf  log
            new AuthMfLogService(token_partner: dto.Token_partner, id_user: dto.Id_user, id_user_log: userlogService.id_user_log, success);

            // Result
            return json;
        }
    }
}

