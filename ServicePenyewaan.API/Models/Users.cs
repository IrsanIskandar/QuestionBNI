namespace ServicePenyewaan.API.Models
{
    public class Users
    {
        public int UserId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string UserEmail { get; set; }
        public string UserPassword { get; set; }
        public string Role { get; set; }
    }
}
