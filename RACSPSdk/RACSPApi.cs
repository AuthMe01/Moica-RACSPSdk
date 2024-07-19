using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tw.moica.RACSPSdk.DataObjects;

namespace tw.moica.RACSPSdk
{
    public class RACSPApi
    {
        private RACSPSdkConfig config { get; set; }

        public RACSPApi(RACSPSdkConfig config)
        {
            this.config = config;
        }


        public async Task<EncryptionCertResponse> EncryptionCertRequest(EncryptionCertRequest request)
        {

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    transaction_id = request.TransactionId,
                    sp_service_id = config.SPServiceId,
                    id_num = request.IdNum,
                    hint = request.Hint,
                    sign_info = new
                    {
                        sign_type = request.SignInfo.SignType,
                        sign_data = Convert.ToBase64String(request.SignInfo.SignData),
                        hash_algorithm = request.SignInfo.HashAlgorithm,
                        tbs_encoding = request.SignInfo.TbsEncoding
                    },
                    op_code = "getEncryptionCert",
                    op_mode = request.OpMode,
                    time_limit = request.TimeLimit,
                    sp_checksum = request.SpCheckSum(config)
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(config.ApiHost + "sp/encCertRequest", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var racResponse = JsonSerializer.Deserialize<EncryptionCertResponse>(responseBody);
                    return racResponse;
                    // Process the response as needed
                }
                else
                {
                    // Handle error response
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return new EncryptionCertResponse()
                    {
                        ErrorCode = ErrorCodes.SERVER_CONNECTION_ERROR,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}: {errorBody}",
                    };
                }
            }
        }
        
        
        public async Task<QueryEncryptionCertResultResponse> QueryEncryptionCertResult(QueryEncryptionCertResultRequest request)
        {

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    transaction_id = request.TransactionId,
                    sp_service_id = config.SPServiceId,
                    sp_ticket_id= request.SpTicketId,
                    sp_checksum = request.SpCheckSum(config)
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(config.ApiHost + "sp/encCertResult", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var racResponse = JsonSerializer.Deserialize<QueryEncryptionCertResultResponse>(responseBody);
                    return racResponse;
                    // Process the response as needed
                }
                else
                {
                    // Handle error response
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return new QueryEncryptionCertResultResponse()
                    {
                        ErrorCode = ErrorCodes.SERVER_CONNECTION_ERROR,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}: {errorBody}",
                    };
                }
            }
        }
        
        public async Task<DecryptionRequestResponse> DecryptionRequest(DecryptionRequest request)
        {
            
            using (var client = new HttpClient())
            {
                var payload = new
                {
                    transaction_id = request.TransactionId,
                    sp_service_id = config.SPServiceId,
                    cert_sn = request.CertSn,
                    hint = request.Hint,
                    encrypted_info = new
                    {
                        encrypted_type = request.EncryptedInfo.EncryptedType,
                        encrypted_data = Convert.ToBase64String(request.EncryptedInfo.EncryptedData),
                        encrypted_encoding = request.EncryptedInfo.EncryptedEncoding
                    },
                    op_code = "decryptionData",
                    op_mode = request.OpMode,
                    time_limit = request.TimeLimit,
                    sp_checksum = request.SpCheckSum(config)
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(config.ApiHost + "sp/decRequest", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var racResponse = JsonSerializer.Deserialize<DecryptionRequestResponse>(responseBody);
                    return racResponse;
                    // Process the response as needed
                }
                else
                {
                    // Handle error response
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return new DecryptionRequestResponse()
                    {
                        ErrorCode = ErrorCodes.SERVER_CONNECTION_ERROR,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}: {errorBody}",
                    };
                }
            }
        }
        
        
        
        public async Task<QueryDecryptionResultResponse> QueryDecryptionResult(QueryDecryptionResultRequest request)
        {

            using (var client = new HttpClient())
            {
                var payload = new
                {
                    transaction_id = request.TransactionId,
                    sp_service_id = config.SPServiceId,
                    sp_ticket_id= request.SpTicketId,
                    sp_checksum = request.SpCheckSum(config)
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(config.ApiHost + "sp/decRequestResult", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var racResponse = JsonSerializer.Deserialize<QueryDecryptionResultResponse>(responseBody);
                    return racResponse;
                    // Process the response as needed
                }
                else
                {
                    // Handle error response
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return new QueryDecryptionResultResponse()
                    {
                        ErrorCode = ErrorCodes.SERVER_CONNECTION_ERROR,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}: {errorBody}",
                    };
                }
            }
        }

    }
}