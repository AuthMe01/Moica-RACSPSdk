using System.ComponentModel.DataAnnotations;
using System.Text;
using tw.moica.RACSPSdk.Utils;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class EncryptedInfo
    {
        public static string SIGN_TYPE_PKCS1 = "PKCS#1";
        public static string SIGN_TYPE_PKCS7 = "PKCS#7";
        
        
        [Required] public string EncryptedType { get; set; }

        [Required] public byte[] EncryptedData { get; set; }
        
        public string EncryptedEncoding { get; set; } = "base64";

        public static byte[] DataWithBase64Text(string signingText)
        {
            return ChecksumTool.ParseBase64UrlSafeString(signingText);
        }
        
        public static byte[] DataWithString(string signingText)
        {
            return Encoding.UTF8.GetBytes(signingText);
        }
    }
}