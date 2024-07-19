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
    public class EncryptionCertRequestTest
    {
        [Fact]
        public async Task TestEncryptionCertRequest()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            var api = new RACSPApi(racspSdkConfig);

            var encryptionCertRequest = new EncryptionCertRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                DeviceUserDefDesc = "",
                Hint = "Helloworld",
                IdNum = "A123679945", //replace with your registered account.
                OpMode = OpModes.PUSH,
                SignInfo = new SignInfo()
                {
                    SignType = SignInfo.SIGN_TYPE_PKCS1,
                    SignData = SignInfo.SignWithText("my little secret with " + racspSdkConfig.SPServiceId),
                },
                TimeLimit = 60
            };
            var res = await api.EncryptionCertRequest(encryptionCertRequest);

            //Requesting end, start validation.
            
            res.ErrorCode.Should().Be("0");
            res.ValidationIDPCheckSum(racspSdkConfig).Should().BeTrue();

            var spTicket = res.Result.ParseSPTicket();
            spTicket.SpTicketId.Should().NotBeNull();
            
            spTicket.TransactionId.Should().Be(encryptionCertRequest.TransactionId);

            // Assertion for IdNum
            spTicket.HashedIdNum.Should().Be(ChecksumTool.GetSHA256inBase64UrlSafe(encryptionCertRequest.IdNum));
            spTicket.OpMode.Should().Be(encryptionCertRequest.OpMode);
            if (!string.IsNullOrEmpty(encryptionCertRequest.DeviceUserDefDesc))
            {
                spTicket.DeviceUserDefDesc.Should().Be(encryptionCertRequest.DeviceUserDefDesc);
            }

            spTicket.SignInfo.SignType.Should().Be(encryptionCertRequest.SignInfo.SignType);
            spTicket.SignInfo.signData.Length.Should().Be(encryptionCertRequest.SignInfo.SignData.Length);

            for (int i = 0; i < spTicket.SignInfo.signData.Length || i < encryptionCertRequest.SignInfo.SignData.Length
                 ; ++i)
            {
                spTicket.SignInfo.signData[i].Should().Be(encryptionCertRequest.SignInfo.SignData[i]);
            }
            
            spTicket.SignInfo.HashAlgorithm.Should().Be(encryptionCertRequest.SignInfo.HashAlgorithm);
            spTicket.SignInfo.TbsEncoding.Should().Be(encryptionCertRequest.SignInfo.TbsEncoding);

            if (encryptionCertRequest.Hint != null)
            {
                spTicket.Hint.Should().Be(encryptionCertRequest.Hint);
            }

        }
    }
}