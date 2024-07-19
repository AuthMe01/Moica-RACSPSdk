using System.ComponentModel.DataAnnotations;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class DecryptionRequest
    {
        [Required]
        public string TransactionId { get; set; }

        [Required]
        public string CertSn { get; set; }

        [Required]
        public string OpMode { get; set; }

        [Required]
        public EncryptedInfo EncryptedInfo { get; set; }

        public string Hint { get; set; }

        public int TimeLimit { get; set; } = 60;


        private string _checkSumRules(RACSPSdkConfig conf)
        {
            return TransactionId + conf.SPServiceId + CertSn;
        }

        public string SpCheckSum(RACSPSdkConfig conf)
        {
            return ChecksumTool.CalculateChecksum(conf, _checkSumRules(conf));
        }
        
    }
}