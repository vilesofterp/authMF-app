
using Newtonsoft.Json.Linq;
using ZionApi;
using ZionOrm;
using Api.Dtos;
using OtpNet;
using System.Text;

namespace Api.Services
{
    public class RegisterService : ZionCrud
    {
        protected JObject json;

        public RegisterService(object request) : base(request)
        {
            serviceName = "Register";
            tableName = "user";
            //dataModel = new UserModel();
            dto = new RegisterDto();
        }
        public JObject Register()
        {
            json = ZionDto.MapperDto(ref dto, base.requestData);

            if (ZionDto.GetMap("status") != "success")
            {
                return ZionResponse.Fail(api_error: 23, body: "Dto Mapper", statusCode: 400, "auto", messageLog: ZionDto.GetResult());
            }

            ZionTotp totp = new ZionTotp();

            //string privateKey = totp.NewPrivateKey();
            string privateKey = "VGtoRmVWRkZhbkF4UVRKdFRreHhVbWhhWkVkRFlXUkVTVlJyUFE9PQ==";

            json.Add("private: ", privateKey);

            string totpCode = totp.GetTotp(privateKey);

            json.Add("totpCode: ", totpCode);

            json.Add("result: ", totp.VerifyTotp(totpCode, privateKey).ToString());

            return json;
        }
    }

    public class ZionTotp
    {
        public byte[] NewPrivateKey()
        {
            long timestamp = DateTime.UtcNow.Ticks;

            var key = KeyGeneration.GenerateRandomKey(20);

            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            for (int i = 0; i < 8; i++)
            {
                key[i] ^= timestampBytes[i % 8];
            }

            return key;
        }

        public string GetTotp(string secretKey)
        {
            byte[] privateKey = Convert.FromBase64String(secretKey);

            var totp = new Totp(privateKey);
            var totpCode = totp.ComputeTotp();
            return totpCode;
        }

        public JObject VerifyTotp(string userEnteredCode, string privateKey)
        {
            var totp = new Totp(Convert.FromBase64String(privateKey));
            bool isValid = totp.VerifyTotp(userEnteredCode, out _, new VerificationWindow(1, 1));

            if (isValid) 
            {
                return ZionResponse.Success("valido");
            }

            return ZionResponse.Fail(51, "", 400, "auto", "Incorrect ou expired Totp Code");
        }

    }
}
