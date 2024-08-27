using System;
using System.ComponentModel.DataAnnotations;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class SignRequest
    {
        [Required] public string TransactionId { get; set; }


        [Required] public string IdNum { get; set; }


        public string DeviceUserDefDesc { get; set; }

        [Required] public SignInfo SignInfo { get; set; }

        public string Hint { get; set; }

        public int TimeLimit { get; set; } = 60;

        private string _checkSumRules(RACSPSdkConfig conf)
        {
            return TransactionId + conf.SPServiceId + IdNum + (DeviceUserDefDesc ?? "") + "SIGN" + Hint +
                   Convert.ToBase64String(SignInfo.SignData);
        }

        public string SpCheckSum(RACSPSdkConfig conf)
        {
            return ChecksumTool.CalculateChecksum(conf, _checkSumRules(conf));
        }
    }
}