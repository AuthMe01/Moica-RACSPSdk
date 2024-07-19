using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using RACSPSdkSample.samples;
using tw.moica.RACSPSdk;
using tw.moica.RACSPSdk.DataObjects;
using tw.moica.RACSPSdk.Utils;
using Xunit;
using Xunit.Abstractions;

namespace RACSPSdkTestSamples.sp
{
    public class DecryptionRequestResultTest
    {
        private readonly ITestOutputHelper _output;
        public DecryptionRequestResultTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task TestDecryptionRequestResult()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            var api = new RACSPApi(racspSdkConfig);


            var queryDecryptionResultRequest = new QueryDecryptionResultRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                SpTicketId = "804f6d2d-db9f-4d0d-a49f-a50f53fd2150"
            };
            var res = await api.QueryDecryptionResult(queryDecryptionResultRequest);

            //Requesting end, start validation.
            
            res.ErrorCode.Should().Be("0");
            res.ValidationIDPCheckSum(racspSdkConfig,queryDecryptionResultRequest).Should().BeTrue();

            _output.WriteLine("Querying Result, Status: "+res.Result.Status);
            
            res.Result.Status.Should().BeOneOf(QueryEncryptionCertResultResponse.STATUS_SUCCESS,
                QueryEncryptionCertResultResponse.STATUS_REJECT,
                QueryEncryptionCertResultResponse.STATUS_REQUESTING,
                QueryEncryptionCertResultResponse.STATUS_EXPIRE);
            //Requesting end, start validation.
                
            res.ValidationIDPCheckSum(racspSdkConfig, queryDecryptionResultRequest)
                .Should().BeTrue();
            res.ErrorCode.Should().Be("0");
            
                        
            if (res.Result.Status == QueryDecryptionResultResponse.STATUS_SUCCESS)
            {
                _output.WriteLine("Querying Result, EncSn:"+res.Result.EncSn);
                _output.WriteLine("Querying Result, Decrypted Data:"+res.Result.DecryptedDataBase64);

                var base64UrlSafeString = ChecksumTool.ParseBase64UrlSafeString(res.Result.DecryptedDataBase64);
                _output.WriteLine("Querying Result, Decrypted Data decode base64 in text :\n" +
                                  Encoding.UTF8.GetString(base64UrlSafeString));

            }

        }
        
    }
}