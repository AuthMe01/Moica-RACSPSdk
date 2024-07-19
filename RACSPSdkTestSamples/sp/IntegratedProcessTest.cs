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
    public class IntegratedProcessTest
    {
        private ITestOutputHelper _output;

        public IntegratedProcessTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Fact]
        public async Task TestAllProcess()
        {
            
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            var api = new RACSPApi(racspSdkConfig);

            var idNum = "A123679945"; //replace with your registered account.
            
            var encryptionCertRequest = new EncryptionCertRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                DeviceUserDefDesc = "",
                Hint = "Helloworld",
                IdNum = idNum,
                OpMode = OpModes.PUSH,
                SignInfo = new SignInfo()
                {
                    SignType = SignInfo.SIGN_TYPE_PKCS1,
                    SignData = SignInfo.SignWithText("my little secret with " + racspSdkConfig.SPServiceId),
                },
                TimeLimit = 60
            };
            
            var encRequestRes = await api.EncryptionCertRequest(encryptionCertRequest);

            encRequestRes.ErrorCode.Should().Be(ErrorCodes.SUCCESS);
            // encRequestRes.ValidationIDPCheckSum(racspSdkConfig).Should().BeTrue();

            var spTicket = encRequestRes.Result.ParseSPTicket();
            spTicket.SpServiceId.Should().NotBeNull();
            
            string spTicketid = spTicket.SpTicketId;


            QueryEncryptionCertResultResponse successResponse = null;
            
            for (int i = 0; i < 10; ++i)
            {
                _output.WriteLine("=========");
                _output.WriteLine("Querying :"+spTicketid);
                
                var encryptionCertResultRequest = new QueryEncryptionCertResultRequest()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SpTicketId = spTicketid
                };
                var res = await api.QueryEncryptionCertResult(encryptionCertResultRequest);

                _output.WriteLine("Querying Result, Status: "+res.Result.Status);
                _output.WriteLine("Querying Result, CertSn:"+res.Result.CertSn);
                _output.WriteLine("Querying Result, Cert:"+res.Result.Cert);
                _output.WriteLine("Querying Result, SignedResponse:"+res.Result.SignedResponse);
                _output.WriteLine("Querying Result, HashedIdNum:"+res.Result.HashedIdNum);
                
                res.ValidationIDPCheckSum(racspSdkConfig, encryptionCertResultRequest)
                    .Should().BeTrue();
                res.ErrorCode.Should().Be("0");

                
                //app approved
                if (res.Result.Status == QueryEncryptionCertResultResponse.STATUS_SUCCESS)
                {
                    successResponse = res;
                    break;
                }
                
                Thread.Sleep(3000);
            }

            successResponse.Should().NotBeNull();

            _output.WriteLine("Start decrpytion test");
            
            
            var decryptionRequest = new DecryptionRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                Hint = "Helloworld",
                CertSn = successResponse!.Result.CertSn,
                OpMode = OpModes.PUSH,
                EncryptedInfo = new EncryptedInfo()
                {
                    EncryptedType = SignInfo.SIGN_TYPE_PKCS1,
                    EncryptedData = EncryptionTool.EncryptPKCS1(
                        successResponse!.Result.Cert,
                        //three type
                        //1. byte[] directly
                        //2. string : EncryptedInfo.DataWithString("Hello world")
                        //3. base64 encoded : EncryptedInfo.DataWithBase64Text("Aa31acae==")
                        EncryptedInfo.DataWithString("Hello world sss")
                    )
                },
                TimeLimit = 60
            };
            var decRes = await api.DecryptionRequest(decryptionRequest);

            //Requesting end, start validation.
            
            decRes.ErrorCode.Should().Be("0");
            decRes.ValidationIDPCheckSum(racspSdkConfig).Should().BeTrue();

            var decSpTicket = decRes.Result.ParseSPTicket();
            decSpTicket.SpTicketId.Should().NotBeNull();


            QueryDecryptionResultResponse decSuccessRes = null;
            
            for (var i = 0; i < 20; ++i)
            {
                
                var queryDecryptionResultRequest = new QueryDecryptionResultRequest()
                {
                    TransactionId = Guid.NewGuid().ToString(),
                    SpTicketId = decSpTicket.SpTicketId
                };
                var res = await api.QueryDecryptionResult(queryDecryptionResultRequest);

                //Requesting end, start validation.
            
                res.ErrorCode.Should().Be("0");
                res.ValidationIDPCheckSum(racspSdkConfig,queryDecryptionResultRequest).Should().BeTrue();

                _output.WriteLine("Querying Result, Status: "+res.Result.Status);
                
                if (res.Result.Status == QueryDecryptionResultResponse.STATUS_SUCCESS)
                {
                    _output.WriteLine("Querying Result, EncSn:"+res.Result.EncSn);
                    _output.WriteLine("Querying Result, Decrypted Data:"+res.Result.DecryptedDataBase64);

                    var base64UrlSafeString = ChecksumTool.ParseBase64UrlSafeString(res.Result.DecryptedDataBase64);
                    _output.WriteLine("Querying Result, Decrypted Data decode base64 in text :\n" +
                                      Encoding.UTF8.GetString(base64UrlSafeString));

                    decSuccessRes = res;
                    break;
                }
                Thread.Sleep(3000);
            }

            decSuccessRes.Should().NotBeNull(); 

        } 
        
        
    }
}