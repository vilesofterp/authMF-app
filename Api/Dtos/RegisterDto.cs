using ZionApi;

namespace Api.Dtos
{
    public class RegisterDto : ZionValidation
    {
        public long Id { get; set; }
        public string Email { get; set; }

        public RegisterDto()
        {
            MapperValidation();
        }

        void MapperValidation()
        {
            Validation["Id"] = new Dictionary<string, object>
            {
                {"MIN_VAL", 1},
            };
            Validation["Email"] = new Dictionary<string, object>
            {
                {"VALID_EMAIL", 1},
            };
        }
    }
}