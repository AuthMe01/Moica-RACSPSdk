using System;

namespace tw.moica.RACSPSdk
{
    public class RACSPSdkConfig
    {
        
        public String ApiHost { get; set; }
        public String SPServiceId { get; set; }
        public String SPApiKey { get; set; }
        
        public byte[] SPApiKeyBytes { get; }
        public RACSPSdkConfig(String apiHost,string spServiceId, string spApiKey)
        {
            ApiHost = apiHost;
            SPServiceId = spServiceId;
            SPApiKey = spApiKey;

            this.SPApiKeyBytes = ParseBase64UrlSafeString(spApiKey);
        }
        
        
        private static byte[] ParseBase64UrlSafeString(string base64UrlSafeString)
        {
            // Replace URL-safe characters with standard Base64 characters
            string base64 = base64UrlSafeString.Replace('-', '+').Replace('_', '/');

            // Add padding if necessary
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            // Decode the Base64 string
            return Convert.FromBase64String(base64);
        }
        
    }
}