namespace ServicePenyewaan.API.Models
{
    public class TokenBased
    {
        public string Access_Token { get; set; }
        public string Expired_Token { get; set; }
        public Users UserDetail { get; set; }
    }
}
