using System;
using System.Text.Json.Serialization;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class GetAthOrSignResultResponse : RACResponse
    {

        [JsonPropertyName("result")] public GetAthOrSignResultResponseResult Result { get; set; }


        public bool ValidationIDPCheckSum(RACSPSdkConfig config,QueryDecryptionResultRequest request)
        {
            var checksum = ChecksumTool.CalculateChecksum(config,
                request.TransactionId,
                ErrorCode, 
                Result.HashedIdNum,
                Result.SignedResponse);

            var checksum2 = Result.IdpChecksum;

            //lets try decrypt them back to the SHA256 of given data.
            var res = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum);
            var res2 = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum2);

            return res == res2;
        }
    }

    public class GetAthOrSignResultResponseResult
    {
        [JsonPropertyName("idp_checksum")] public string IdpChecksum { get; set; }

        [JsonPropertyName("cert")] public string Cert { get; set; }
        [JsonPropertyName("hashed_id_num")] public string HashedIdNum { get; set; }
        [JsonPropertyName("signed_response")] public string SignedResponse { get; set; }
    }
}