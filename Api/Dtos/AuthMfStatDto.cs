
using ZionApi;

namespace Api.Dtos
{
    public class AuthMfStatDto : ZionValidation
    {
        public long id { get; set; }
        public long id_partner { get; set; }
        public int type_record { get; set; }
        public string key_record { get; set; }
        public long requests { get; set; }
        public AuthMfStatDto()
        {
            MapperValidation();
        }
        void MapperValidation()
        {
            Validation["Id_user"] = new Dictionary<string, object>
            {
                {"MIN_VAL", 1},
            };

            Validation["Id_partner"] = new Dictionary<string, object>
            {
                {"MIN_VAL", 1},
            };
        }
    }
}
