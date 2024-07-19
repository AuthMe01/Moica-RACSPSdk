using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class QueryDecryptionResultRequest
    {
        public string TransactionId { get; set; }

        public string SpTicketId { get; set; }
        
        private string _checkSumRules(RACSPSdkConfig conf)
        {
            return TransactionId + conf.SPServiceId + SpTicketId;
        }

        public string SpCheckSum(RACSPSdkConfig conf)
        {
            return ChecksumTool.CalculateChecksum(conf, _checkSumRules(conf));
        }

    }
}