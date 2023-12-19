using ZionApi;

namespace Api.Dtos
{
    public class ValidateTotpDto : ZionValidation
    {
        public long Id_user { get; set; }
        public string Email { get; set; }
        public string TotpCodel { get; set; }

        public ValidateTotpDto()
        {
            MapperValidation();
        }

        void MapperValidation()
        {
            Validation["Id_user"] = new Dictionary<string, object>
            {
                {"MIN_VAL", 1},
            };

            Validation["Email"] = new Dictionary<string, object>
            {
                {"VALID_EMAIL", 1},
            };

            Validation["TotpCode"] = new Dictionary<string, object>
            {
                {"MIN_LEN", 6},
                {"MAX_LEN", 6},
            };
        }
    }
}