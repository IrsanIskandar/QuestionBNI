namespace ServicePenyewaan.API.Models.Response
{
    public class ResponLoginViewModel
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public TokenBased Data { get; set; }
    }
}
