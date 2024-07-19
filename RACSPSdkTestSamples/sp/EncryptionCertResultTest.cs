using System;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using RACSPSdkSample.samples;
using tw.moica.RACSPSdk;
using tw.moica.RACSPSdk.DataObjects;
using Xunit;
using Xunit.Abstractions;

namespace RACSPSdkTestSamples.sp
{
    public class EncryptionCertResultTest
    {
        private readonly ITestOutputHelper output;
        public EncryptionCertResultTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public async Task TestEncryptionCertResultQuery()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            var api = new RACSPApi(racspSdkConfig);

            
            //you have to change the ticket id to the sp_ticket_id you got from 
            //  EncryptionCertResponseResult (#see RACSPApi#EncryptionCertRequest for more details )
            var spTicketid = "53a87d35-53b0-4ae0-932a-2006a1cfae52";
            
            var encryptionCertResultRequest = new QueryEncryptionCertResultRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                SpTicketId = spTicketid
            };
            var res = await api.QueryEncryptionCertResult(encryptionCertResultRequest);

            res.Result.Status.Should().BeOneOf(QueryEncryptionCertResultResponse.STATUS_SUCCESS,
                QueryEncryptionCertResultResponse.STATUS_REJECT,
                QueryEncryptionCertResultResponse.STATUS_REQUESTING,
                QueryEncryptionCertResultResponse.STATUS_EXPIRE);
            //Requesting end, start validation.

            res.ValidationIDPCheckSum(racspSdkConfig, encryptionCertResultRequest)
                .Should().BeTrue();
            res.ErrorCode.Should().Be("0");
        }
        
        
        [Fact]
        public async Task TestEncryptionCertResultQueryLoop()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            var api = new RACSPApi(racspSdkConfig);

            
            //you have to change the ticket id to the sp_ticket_id you got from 
            //  EncryptionCertResponseResult (#see RACSPApi#EncryptionCertRequest for more details )
            var spTicketid = "53a87d35-53b0-4ae0-932a-2006a1cfae52";

            for (int i = 0; i < 10; ++i)
            {
                output.WriteLine("=========");
                output.WriteLine("Querying :"+spTicketid);
                
                var encryptionCertResultRequest = new QueryEncryptionCertResultRequest()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SpTicketId = spTicketid
                };
                var res = await api.QueryEncryptionCertResult(encryptionCertResultRequest);

                output.WriteLine("Querying Result, Status: "+res.Result.Status);
                output.WriteLine("Querying Result, CertSn:"+res.Result.CertSn);
                output.WriteLine("Querying Result, Cert:"+res.Result.Cert);
                output.WriteLine("Querying Result, SignedResponse:"+res.Result.SignedResponse);
                output.WriteLine("Querying Result, HashedIdNum:"+res.Result.HashedIdNum);
                
                res.Result.Status.Should().BeOneOf(QueryEncryptionCertResultResponse.STATUS_SUCCESS,
                    QueryEncryptionCertResultResponse.STATUS_REJECT,
                    QueryEncryptionCertResultResponse.STATUS_REQUESTING,
                    QueryEncryptionCertResultResponse.STATUS_EXPIRE);
                //Requesting end, start validation.

                
                res.ValidationIDPCheckSum(racspSdkConfig, encryptionCertResultRequest)
                    .Should().BeTrue();
                res.ErrorCode.Should().Be("0");
                
                Thread.Sleep(3000);
                
            }
        }
    }
}