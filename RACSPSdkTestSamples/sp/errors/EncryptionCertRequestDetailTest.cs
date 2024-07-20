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
    public class EncryptionCertRequestDetailTest
    {
        private RACSPApi _api;

        [Fact]
        public async Task TestPMInv()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            _api = new RACSPApi(racspSdkConfig);

            var encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.TransactionId = "";
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");

            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.IdNum = "";
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");

            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.OpMode = "";
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");


            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.SignInfo = null;
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");

            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.SignInfo.SignType = null;
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");

            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.SignInfo.SignData = null;
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");
        
            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.OpMode = "Test";
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");
            
            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.SignInfo.SignType = "Test";
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "ENC_REQUEST_CERT_SIGN_TYPE_ERROR");
            
            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.TimeLimit = 1000;
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");
            
            encryptionCertRequest = GenEncryptionCertRequest(_api);
            encryptionCertRequest.TimeLimit = -20;
            await RunPMINF(encryptionCertRequest, racspSdkConfig, "PM_INV_NF");
        }

        private async Task RunPMINF(EncryptionCertRequest encryptionCertRequest, RACSPSdkConfig racspSdkConfig, string errorCode)
        {
            var res = await _api.EncryptionCertRequest(encryptionCertRequest);
            res.ErrorCode.Should().Be(errorCode);
        }

        private static EncryptionCertRequest GenEncryptionCertRequest(RACSPApi api)
        {
            return new EncryptionCertRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                DeviceUserDefDesc = "",
                Hint = "Helloworld",
                IdNum = MockKeyProvider.IdNum, //replace with your registered account.
                OpMode = OpModes.PUSH,
                SignInfo = new SignInfo()
                {
                    SignType = SignInfo.SIGN_TYPE_PKCS1,
                    SignData = SignInfo.SignWithText("my little secret with " + api.spServiceId),
                },
                TimeLimit = 60
            };
        }
    }
}