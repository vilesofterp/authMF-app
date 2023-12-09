namespace Api.Models
{
    public class UserModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string NickName { get; set; }
        public string Email { get; set; }
        public string Email_alternative { get; set; }
        public string Password { get; set; }
        public string Gender { get; set; }
        public string Language { get; set; }
        public int Active { get; set; }
        public long Id_time_zone { get; set; }
        public string Phone { get; set; }
        public int Profile { get; set; }
        public int MFA { get; set; }
        public DateTime Global_logout { get; set; }
    }
}
