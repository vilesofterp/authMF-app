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
           // RegisterDto dto = new RegisterDto();

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

            if (GetJson("status") == "success")
            {
                new UserLogService(
                       id_user: dto.Id_user,
                       code: 3,
                       operation: "register app",
                       latitude: dto.Latitude,
                       longitude: dto.Longitude,
                       altitude: dto.Altitude,
                       ip: dto.Ip
                   );
            }

            return json;

        }
    }
}

