using ZionApi;

namespace Api.Dtos
{
    public class GetPartnersDto : ZionValidation
    {
        public long Id_user { get; set; }
   
        public GetPartnersDto()
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