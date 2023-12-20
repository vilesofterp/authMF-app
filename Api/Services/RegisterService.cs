using Api.Dtos;
using ZionOrm;
using ModelVAS;
using ZionHelper;
using VasLog.Services;

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

        public dynamic Register()
        {
            // Flow Crud
            ZionModel model = new ZionModel(dataModel);
            model.Condition = "id = @id_user@ and email = @email@ and deleted = 0 and active = 1";
            string UnMappedFields = "token_auth_mf";
            json = Flow(ref model, UnMappedFields);

            if (GetJson("status") != "success")
            {
                return json;
            }

            // Edit data
            ZionTotp totp = new ZionTotp();
            model.RecordNew.token_auth_mf = ZionSecurity.Mask(totp.NewPrivateKey(), 64);

            // Save data
            model.ResponseFields = "@token_auth_mf";
            json = model.Save(serviceName);
            int success = GetJson("status") == "success" ? 1 : 0;
            int userLogCode = success == 1 ? 3 : 25;

            // user log
            new UserLogService(
                id_user: dto.Id_user,
                code: userLogCode,
                operation: "",
                latitude: dto.Latitude,
                longitude: dto.Longitude,
                altitude: dto.Altitude,
                ip: dto.Ip
            );

            // Auth_mf  log
            new AuthMfLogService(token_partner: dto.Token_partner, id_user: dto.Id_user, success);

            // Result
            return json;

        }
    }
}

