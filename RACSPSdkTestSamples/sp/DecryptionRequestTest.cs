using System;
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
    public class DecryptionRequestTest
    {
        private readonly ITestOutputHelper _output;
        public DecryptionRequestTest(ITestOutputHelper output)
        {
            this._output = output;
        }

        [Fact]
        public async Task TestDecryptionRequest()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            var api = new RACSPApi(racspSdkConfig);


            //get this information from QueryEncryptionCertResult#cert 
            string certBase64 = "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";

            var decryptionRequest = new DecryptionRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                Hint = "Helloworld",
                CertSn = "5CDD0D3A0A9A356F464B9EA110ACB5C2",
                OpMode = OpModes.PUSH,
                EncryptedInfo = new EncryptedInfo()
                {
                    EncryptedType = SignInfo.SIGN_TYPE_PKCS1,
                    EncryptedData = EncryptionTool.EncryptPKCS1(certBase64,
                        //three type
                        //1. byte[] directly
                        //2. string : EncryptedInfo.DataWithString("Hello world")
                        //3. base64 encoded : EncryptedInfo.DataWithBase64Text("Aa31acae==")
                        EncryptedInfo.DataWithString("Hello world")
                    )
                },
                TimeLimit = 60
            };
            var res = await api.DecryptionRequest(decryptionRequest);

            //Requesting end, start validation.
            
            res.ErrorCode.Should().Be("0");
            res.ValidationIDPCheckSum(racspSdkConfig).Should().BeTrue();

            var spTicket = res.Result.ParseSPTicket();
            spTicket.SpTicketId.Should().NotBeNull();
            
            _output.WriteLine("sp_ticket_id:"+spTicket.SpTicketId);
            
            spTicket.TransactionId.Should().Be(decryptionRequest.TransactionId);

            // Assertion for IdNum
            // spTicket.HashedIdNum.Should().Be(ChecksumTool.GetSHA256inBase64UrlSafe("youridnum"));
            spTicket.OpMode.Should().Be(decryptionRequest.OpMode);
            spTicket.CertSn.Should().Be(decryptionRequest.CertSn);

            if (decryptionRequest.Hint != null)
            {
                spTicket.Hint.Should().Be(decryptionRequest.Hint);
            }

        }
        
    }
}