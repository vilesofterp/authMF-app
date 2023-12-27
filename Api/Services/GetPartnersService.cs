using Api.Dtos;
using ZionOrm;
using ModelVAS;
using ZionHelper;
using VasLog.Services;
using System.Data;
using Newtonsoft.Json.Linq;
using ZionApi;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Security.Cryptography.Xml;
using System;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Security.Cryptography.X509Certificates;

namespace Api.Services
{
    public class GetPartnersService : ZionCrud
    {
        public GetPartnersService(object request) : base(request)
        {
            serviceName = "GetPartners";
            tableName = "partner";
            dataModel = new PartnerModel();
            dto = new GetPartnersDto();
        }

        public dynamic GetPartners()
        {
            UserService userService = new(requestData);
            json = userService.LoadUser("id = @id_user@ and email = @email@ and deleted = 0 and active = 1");

            sqlQuery = "SELECT p.id, p.updated_at, c.name, c.nickname FROM partner p, company c WHERE p.id_company = c.id  AND p.deleted = 0  AND c.deleted = 0  AND p.active = 1 AND c.active = 1"; 
            ResponseFields = "name, nickname, @token, updated_at";
            json = Get();
            int records = ZionConv.ToInt(GetJsonSummary("records_json"));

            if (records > 0)
            {
                ZionTotp totp = new ZionTotp();
                JObject response = new JObject();
                Orm orm = new Orm();

                for (int i = 1; i <= records; i++)
                {
                    string mf_id_user = dto.Id_user.ToString();
                    string mf_id_partner = GetJsonRecord(i, "id");
                    string mf_partner_name = GetJsonRecord(i, "name");
                    string mf_partner_nickname = GetJsonRecord(i, "nickname");
                    string mf_partner_updated_at = GetJsonRecord(i, "updated_at");
                    string mf_public_key = ZionSecurity.Mask(totp.NewPrivateKey(), 64);

                    ZionEncrypter hash = new ZionEncrypter();
                    string hashKey = mf_id_user + mf_id_partner + mf_public_key;
                    string mf_private_key = hash.CreateHash(hashKey);

                    string push =
                        "insert into auth_mf (id_user, id_partner, public_key, private_key, active) values(" +
                        SqlPar(mf_id_user) + ", " +
                        SqlPar(mf_id_partner) + ", " +
                        SqlPar(mf_public_key) + ", " +
                        SqlPar(mf_private_key) +
                        ", 1) on conflict(id_user, id_partner) do update set public_key = " + SqlPar(mf_public_key) + ", private_key = " + SqlPar(mf_private_key); 
                    orm.SetPush(push);

                    var register = new JObject()
                    {
                        { "partner_id", mf_id_partner },
                        { "partner_name", mf_partner_name },
                        { "partner_nickname", mf_partner_nickname },
                        { "partner_updated_at", mf_partner_updated_at },
                        { "partner_public_key", mf_public_key }
                    };

                    response.Add(i.ToString(), register);
                }

                orm.PushExec();

                json = ZionResponse.Success(response);
            } 
            else
            {
                json = ZionResponse.Fail(60, "incorrect parameters", 400, "auto", "No Partner was found with the data received in the request: " + LastSqlSentence);
            }

            int success = GetJson("status") == "success" ? 1 : 0;
            int userLogCode = success == 1 ? 3 : 25;

            // user log
            UserLogService userlogService = new UserLogService(
                id_user: dto.Id_user,
                code: userLogCode,
                operation: "",
                latitude: dto.Latitude,
                longitude: dto.Longitude,
                altitude: dto.Altitude,
                ip: dto.Ip
            );

            // Auth_mf  log
            new AuthMfLogService(token_partner: dto.Token_partner, id_user: dto.Id_user, id_user_log: userlogService.id_user_log, success);

            // Result
            return json;

        }
    }
}

