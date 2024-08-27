using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using tw.moica.RACSPSdk.DataObjects;

namespace tw.moica.RACSPSdk
{
    public partial class RACSPApi
    {
        public async Task<SignResponse> Sign(SignRequest request)
        {
            using (var client = new HttpClient())
            {
                string signData = null;

                if (request.SignInfo != null && request.SignInfo.SignData != null)
                {
                    signData = Convert.ToBase64String(request.SignInfo.SignData);
                }

                string json = null;
                
                //TODO: refine this
                if (string.IsNullOrEmpty(request.DeviceUserDefDesc))
                {
                    var payload = new
                    {
                        transaction_id = request.TransactionId,
                        sp_service_id = config.SPServiceId,
                        id_num = request.IdNum,
                        hint = request.Hint,
                        sign_info = new
                        {
                            sign_type = request.SignInfo?.SignType,
                            sign_data = signData,
                            hash_algorithm = request.SignInfo?.HashAlgorithm,
                            tbs_encoding = request.SignInfo?.TbsEncoding
                        },
                        op_code = "SIGN",   
                        time_limit = request.TimeLimit,
                        sp_checksum = request.SpCheckSum(config)
                    };
                    json = JsonSerializer.Serialize(payload);
                }
                else
                {
                    var payload = new
                    {
                        transaction_id = request.TransactionId,
                        sp_service_id = config.SPServiceId,
                        id_num = request.IdNum,
                        hint = request.Hint,
                        sign_info = new
                        {
                            sign_type = request.SignInfo?.SignType,
                            sign_data = signData,
                            hash_algorithm = request.SignInfo?.HashAlgorithm,
                            tbs_encoding = request.SignInfo?.TbsEncoding
                        },
                        device_user_def_desc = request.DeviceUserDefDesc,
                        op_code = "SIGN",   
                        time_limit = request.TimeLimit,
                        sp_checksum = request.SpCheckSum(config)
                    };
                    json = JsonSerializer.Serialize(payload);
                }
                
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(config.ApiHost + "sp/requestAthOrSignPush", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var racResponse = JsonSerializer.Deserialize<SignResponse>(responseBody);
                    return racResponse;
                    // Process the response as needed
                }
                else
                {
                    // Handle error response
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return new SignResponse()
                    {
                        ErrorCode = ErrorCodes.SERVER_CONNECTION_ERROR,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}: {errorBody}",
                    };
                }
            }
        }
        
        public async Task<GetAthOrSignResultResponse> GetAthOrSignResult(
            GetAthOrSignResultRequest request)
        {
            using (var client = new HttpClient())
            {
                var payload = new
                {
                    transaction_id = request.TransactionId,
                    sp_service_id = config.SPServiceId,
                    sp_ticket_id = request.SpTicketId,
                    sp_checksum = request.SpCheckSum(config)
                };

                var json = JsonSerializer.Serialize(payload);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(config.ApiHost + "sp/getAthOrSignResult", content);

                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var racResponse = JsonSerializer.Deserialize<GetAthOrSignResultResponse>(responseBody);
                    return racResponse;
                    // Process the response as needed
                }
                else
                {
                    // Handle error response
                    var errorBody = await response.Content.ReadAsStringAsync();

                    return new GetAthOrSignResultResponse()
                    {
                        ErrorCode = ErrorCodes.SERVER_CONNECTION_ERROR,
                        ErrorMessage = $"Request failed with status code {response.StatusCode}: {errorBody}",
                    };
                }
            }
        }


    }
}