using ZionApi;

namespace Api.Dtos
{
    public class RegisterDto : ZionValidation
    {
        public double Id_user { get; set; }
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
        }
    }
}