using System;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class SignResponse : RACResponse
    {
        [JsonPropertyName("result")] public SignResponseResult Result { get; set; }

        
        public bool ValidationIDPCheckSum(RACSPSdkConfig config)
        {
            var spticket = Result.ParseSPTicket();
            var checksum = ChecksumTool.CalculateChecksum(config, spticket.TransactionId,
                ErrorCode, Result.SpTicket);

            var checksum2 = Result.IdpChecksum;

            //lets try decrypt them back to the SHA256 of given data.
            var res = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum);
            var res2 = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config, checksum2);

            return res == res2;
        }
    }

    // ReSharper disable once ClassNeverInstantiated.Global
    public class SignResponseResult
    {
        [JsonPropertyName("sp_ticket")] public string SpTicket { get; set; }
        [JsonPropertyName("idp_checksum")] public string IdpChecksum { get; set; }


        public SignSPTicket ParseSPTicket()
        {
            if (string.IsNullOrEmpty(SpTicket))
            {
                throw new ArgumentException("sp ticket didn't fetch correctly ");
            }

            var tokens = SpTicket.Split(".");
            var decodeToObject = Encoding.UTF8.GetString(ChecksumTool.ParseBase64UrlSafeString(tokens[0]));
            var ticket = JsonSerializer.Deserialize<SignSPTicket>(decodeToObject);

            
            //try decode, if decode fail, we just leave it as original we get.
            try
            {
                ticket.SpName = Encoding.UTF8.GetString(ChecksumTool.ParseBase64UrlSafeString(ticket.SpName));
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
            }

            //try decode, if decode fail, we just leave it as original we get.
            try
            {
                ticket.Hint = Encoding.UTF8.GetString(ChecksumTool.ParseBase64UrlSafeString(ticket.Hint));
            }
            // ReSharper disable once EmptyGeneralCatchClause
            catch (Exception ex)
            {
            }

            return ticket;
        }
    }

    public class SignSPTicket
    {
        [JsonPropertyName("sp_name")] public string SpName { get; set; }

        [JsonPropertyName("transaction_id")] public string TransactionId { get; set; }

        [JsonPropertyName("hint")] public string Hint { get; set; }

        [JsonPropertyName("sp_ticket_id")] public string SpTicketId { get; set; }

        [JsonPropertyName("op_code")] public string OpCode { get; set; } = "getEncryptionCert";

        [JsonPropertyName("sp_service_id")] public string SpServiceId { get; set; }

        [JsonPropertyName("op_mode")] public string OpMode { get; set; }

        [JsonPropertyName("hashed_id_num")] public string HashedIdNum { get; set; }

        [JsonPropertyName("expiration_time")] public string ExpirationTime { get; set; }

        [JsonPropertyName("device_user_def_desc")]
        public string DeviceUserDefDesc { get; set; }

        [JsonPropertyName("sign_info")] public SignInfoResponse SignInfo { get; set; }
    }
}