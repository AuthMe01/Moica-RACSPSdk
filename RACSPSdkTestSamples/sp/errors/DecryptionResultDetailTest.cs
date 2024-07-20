﻿using System;
using System.Threading.Tasks;
using FluentAssertions;
using RACSPSdkSample.samples;
using tw.moica.RACSPSdk;
using tw.moica.RACSPSdk.DataObjects;
using tw.moica.RACSPSdk.Utils;
using Xunit;

namespace RACSPSdkTestSamples.sp
{
    public class DecryptionResultDetailTest
    {
        private RACSPApi _api;

        [Fact]
        public async Task TestPMInv()
        {
            //please replace the service id and apikey with the one you got from tw MOICA service provider.
            var racspSdkConfig = new RACSPSdkConfig(MockKeyProvider.TestAPIHost, MockKeyProvider.ServiceId,
                MockKeyProvider.ApiKey);
            _api = new RACSPApi(racspSdkConfig);

            var requestObject = GenRequestObject(_api);
            requestObject.TransactionId = "";
            await RunPMINF(requestObject, "PM_INV_NF");

            requestObject = GenRequestObject(_api);
            requestObject.SpTicketId = "";
            await RunPMINF(requestObject, "PM_INV_NF");
            
            
            requestObject = GenRequestObject(_api);
            requestObject.SpTicketId = "This is not exist";
            await RunPMINF(requestObject, "SPSVCTICKET_NF");
            
        }

        private async Task RunPMINF(QueryDecryptionResultRequest encryptionCertRequest, string errorCode)
        {
            var res = await _api.QueryDecryptionResult(encryptionCertRequest);
            res.ErrorCode.Should().Be(errorCode);
        }

        private static QueryDecryptionResultRequest GenRequestObject(RACSPApi api)
        {
            return new QueryDecryptionResultRequest()
            {
                TransactionId = Guid.NewGuid().ToString(),
                SpTicketId = "aaa"
            };
        }
    }
}