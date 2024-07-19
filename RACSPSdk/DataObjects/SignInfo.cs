using System.ComponentModel.DataAnnotations;
using System.Text;

namespace tw.moica.RACSPSdk.DataObjects
{
    public class SignInfo
    {
        public static string SIGN_TYPE_PKCS1 = "PKCS#1";
        public static string SIGN_TYPE_PKCS7 = "PKCS#7";
        
        
        [Required] public string SignType { get; set; }

        [Required] public byte[] SignData { get; set; }

        public string HashAlgorithm { get; set; } = "SHA256";

        public string TbsEncoding { get; set; } = "base64";

        public static byte[] SignWithText(string signingText)
        {
            byte[] byteArray = Encoding.UTF8.GetBytes(signingText);
            return byteArray;
        }
    }
}