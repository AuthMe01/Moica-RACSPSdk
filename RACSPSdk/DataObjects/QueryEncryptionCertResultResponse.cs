using System.Text.Json.Serialization;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class QueryEncryptionCertResultResponse : RACResponse
    {
        public const string STATUS_REQUESTING = "requesting";
        public const string STATUS_SUCCESS = "success";
        public const string STATUS_REJECT = "reject";
        public const string STATUS_EXPIRE = "expire";

        [JsonPropertyName("result")] public QueryEncryptionCertResultResponseResult Result { get; set; }


        public bool ValidationIDPCheckSum(RACSPSdkConfig config,QueryEncryptionCertResultRequest request)
        {
            var checksum = ChecksumTool.CalculateChecksum(config,
                request.TransactionId,
                ErrorCode, 
                Result.HashedIdNum,
                Result.CertSn,
                Result.SignedResponse);

            var checksum2 = Result.IdpChecksum;

            //lets try decrypt them back to the SHA256 of given data.
            var res = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum);
            var res2 = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum2);

            return res == res2;
        }
    }

    public class QueryEncryptionCertResultResponseResult
    {
        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("idp_checksum")] public string IdpChecksum { get; set; }

        [JsonPropertyName("cert_sn")] public string CertSn { get; set; }
        [JsonPropertyName("cert")] public string Cert { get; set; }
        [JsonPropertyName("hashed_id_num")] public string HashedIdNum { get; set; }
        [JsonPropertyName("signed_response")] public string SignedResponse { get; set; }
    }
}