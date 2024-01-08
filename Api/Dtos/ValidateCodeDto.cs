using ZionApi;

namespace Api.Dtos
{
    public class ValidateCodeDto : ZionValidation
    {
        public long Id_user { get; set; }
        public string Code { get; set; }

        public ValidateCodeDto()
        {
            MapperValidation();
        }

        void MapperValidation()
        {
            Validation["Id_user"] = new Dictionary<string, object>
            {
                {"MIN_VAL", 1},
            };

            Validation["Code"] = new Dictionary<string, object>
            {
                {"MIN_LEN", 6},
                {"MAX_LEN", 6},
            };
        }
    }
}