using System.ComponentModel.DataAnnotations;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class EncryptionCertRequest
    {
        [Required]
        public string TransactionId { get; set; }
        

        [Required]
        public string IdNum { get; set; }

        [Required]
        public string OpMode { get; set; }

        public string DeviceUserDefDesc { get; set; }

        [Required]
        public SignInfo SignInfo { get; set; }

        public string Hint { get; set; }

        public int TimeLimit { get; set; } = 60;


        private string _checkSumRules(RACSPSdkConfig conf)
        {
            return TransactionId + conf.SPServiceId + IdNum;
        }

        public string SpCheckSum(RACSPSdkConfig conf)
        {
            return ChecksumTool.CalculateChecksum(conf, _checkSumRules(conf));
        }
        
    }
}