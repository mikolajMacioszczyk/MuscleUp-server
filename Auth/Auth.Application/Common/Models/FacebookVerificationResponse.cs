namespace Auth.Application.Common.Models
{
    public class FacebookVerificationResponse
    {
        public FacebookVerificationResponseData data { get; set; }
    }

    public class FacebookVerificationResponseData
    {
        public string app_id { get; set; }
        public string type { get; set; }
        public string application { get; set; }
        public int data_access_expires_at { get; set; }
        public int expires_at { get; set; }
        public bool is_valid { get; set; }
        public IEnumerable<string> scopes { get; set; }
        public string user_id { get; set; }
    }
}
