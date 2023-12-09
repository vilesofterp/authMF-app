using ZionApi;

namespace Api.Dtos
{
    public class RegisterDto : ZionValidation
    {
        public double Id { get; set; }
        public string Email { get; set; }
        public string token_auth_private { get; set; }
        public RegisterDto()
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
        }
    }
}