using System.Text.Json.Serialization;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class SignInfoResponse
    {
        [JsonPropertyName("sign_type")]
        public string SignType { get; set; }

        [JsonPropertyName("sign_data")]
        public string SignDataBase64 { get; set; }

        public byte[] signData => ChecksumTool.ParseBase64UrlSafeString(SignDataBase64);

        [JsonPropertyName("hash_algorithm")]
        public string HashAlgorithm { get; set; } = "SHA256";

        [JsonPropertyName("tbs_encoding")]
        public string TbsEncoding { get; set; } = "base64";

    }
}