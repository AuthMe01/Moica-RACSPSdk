using System;
using System.Threading.Tasks;
using FluentAssertions;
using RACSPSdkSample.samples;
using tw.moica.RACSPSdk;
using tw.moica.RACSPSdk.DataObjects;
using tw.moica.RACSPSdk.Utils;
using Xunit;

namespace RACSPSdkTestSamples.sp
{
    public class DecryptionRequestDetailTest
    {
        private RACSPApi _api;

        [Fact]
        public async Task TestPMInv()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            _api = new RACSPApi(racspSdkConfig);

            var decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.TransactionId = "";
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");

            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.CertSn = "";
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");

            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.OpMode = "";
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");


            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.EncryptedInfo = null;
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");

            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.EncryptedInfo.EncryptedType = null;
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");

            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.EncryptedInfo.EncryptedData = null;
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");
        
            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.OpMode = "Test";
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");
            
            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.EncryptedInfo.EncryptedType = "Test";
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");
            
            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.TimeLimit = 1000;
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");
            
            decryptCertRequest = DecryptCertRequest(_api);
            decryptCertRequest.TimeLimit = -20;
            await RunPMINF(decryptCertRequest, racspSdkConfig, "PM_INV_NF");
        }

        private async Task RunPMINF(DecryptionRequest encryptionCertRequest, RACSPSdkConfig racspSdkConfig, string errorCode)
        {
            var res = await _api.DecryptionRequest(encryptionCertRequest);
            res.ErrorCode.Should().Be(errorCode);
        }

        private static DecryptionRequest DecryptCertRequest(RACSPApi api)
        {
            return new DecryptionRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                CertSn = MockKeyProvider.EncCertSN,
                Hint = "Helloworld",
                OpMode = OpModes.PUSH,
                EncryptedInfo = new EncryptedInfo()
                {
                    EncryptedType = EncryptedInfo.SIGN_TYPE_PKCS1,
                    EncryptedData = 
                        EncryptionTool.EncryptPKCS1(MockKeyProvider.EncCert,
                            //three type
                            //1. byte[] directly
                            //2. string : EncryptedInfo.DataWithString("Hello world")
                            //3. base64 encoded : EncryptedInfo.DataWithBase64Text("Aa31acae==")
                            EncryptedInfo.DataWithString("Hello world")
                        )
                },
                TimeLimit = 60
            };
        }
    }
}