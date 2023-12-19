﻿using Api.Dtos;
using ZionOrm;
using ModelVAS;
using ZionHelper;
using VasLog.Services;

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
            // Flow Crud
            ZionModel model = new ZionModel(dataModel);
            model.Condition = "id = @id_user@ and email = @email@ and deleted = 0 and active = 1";
            json = Flow(ref model);

            if (GetJson("status") != "success")
            {
                return json;
            }

            // Code Validation
            ZionTotp totp = new ZionTotp();
            json = totp.VerifyTotp(dto.Code, ZionSecurity.UnMask(model.RecordNew.token_auth_mf, 64));
            int userLogCode = GetJson("status") == "success"  ? 23 : 24;

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

            return json;
        }
    }
}
