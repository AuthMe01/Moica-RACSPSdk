using System;
using System.Text.Json.Serialization;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class QueryDecryptionResultResponse : RACResponse
    {
        public const string STATUS_REQUESTING = "requesting";
        public const string STATUS_SUCCESS = "success";
        public const string STATUS_REJECT = "reject";
        public const string STATUS_EXPIRE = "expire";

        [JsonPropertyName("result")] public QueryDecryptionResultResponseResult Result { get; set; }


        public bool ValidationIDPCheckSum(RACSPSdkConfig config,QueryDecryptionResultRequest request)
        {
            var checksum = ChecksumTool.CalculateChecksum(config,
                request.TransactionId,
                ErrorCode, 
                Result.EncSn,
                Result.Status,
                Result.DecryptedDataBase64);

            var checksum2 = Result.IdpChecksum;

            //lets try decrypt them back to the SHA256 of given data.
            var res = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum);
            var res2 = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum2);

            return res == res2;
        }
    }

    public class QueryDecryptionResultResponseResult
    {
        [JsonPropertyName("status")] public string Status { get; set; }
        [JsonPropertyName("idp_checksum")] public string IdpChecksum { get; set; }

        [JsonPropertyName("enc_sn")] public string EncSn { get; set; }
        
        [JsonPropertyName("decrypted_data")] 
        public string DecryptedDataBase64 { get; set; }
    }
}