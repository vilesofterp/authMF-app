
using ZionApi;

namespace Api.Dtos
{
    public class AuthMfLogDto : ZionValidation
    {
        public long id { get; set; }
        public long id_partner { get; set; }
        public long id_user { get; set; }
        public long id_user_log { get; set; }
        public DateTime date { get; set; }
        public int success { get; set; }
        public AuthMfLogDto()
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

            Validation["Id_user_log"] = new Dictionary<string, object>
            {
                {"MIN_VAL", 1},
            };

            Validation["success"] = new Dictionary<string, object>
            {
                {"LOGICAL", 1},
            };
        }
    }
}

