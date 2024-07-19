using System;
using FluentAssertions;
using tw.moica.RACSPSdk;
using tw.moica.RACSPSdk.Utils;
using Xunit;

namespace RACSPSdkSample.samples.checksums
{
    public class ChecksumChecksTest
    {
        
        [Fact]
        public void Execute()
        {
            var config = new RACSPSdkConfig(MockKeyProvider.TestAPIHost,MockKeyProvider.ServiceId,MockKeyProvider.ApiKey);

            //Try generate a checksum with given rule
            // transaction_id+sp_service_id+id_num
            
            // prepare test data start
            var transaction_id = "41570c02-5226-4d2e-8391-839ed31123cf";
            var id_num = "A123456789";
            // prepare test data end
            
            
            var checksum = ChecksumTool.CalculateChecksum(config, transaction_id,
                config.SPServiceId, id_num);
            
            var checksum2 = ChecksumTool.CalculateChecksum(config, transaction_id,
                config.SPServiceId, id_num);

            
            //two checksum should be different since we use random iv in gcm.
            checksum.Should().NotBe(checksum2);
            
            
            //lets try decrypt them back to the SHA256 of given data.
            var res = ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config,checksum);
            var res2 =  ChecksumTool.DecryptWithAesGcmRandomIvInBytes(config,checksum2);
            
            //The SHA 256 should be the same.
            res.Should().Be(res2);


        }
        
    }
}