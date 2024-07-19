using System;
using System.Security.Cryptography;
using System.Security.Cryptography.Pkcs;
using System.Security.Cryptography.X509Certificates;

namespace tw.moica.RACSPSdk.Utils
{
    public class EncryptionTool
    {

        public static byte[] EncryptPKCS1(string certBase64, byte[] data)
        {
            return EncryptPKCS1(ChecksumTool.ParseBase64UrlSafeString(certBase64),data);
        }

        public static byte[] EncryptPKCS1(byte[] cert,byte[] data)
        {
            var certificate = new X509Certificate2(cert);

            using (var rsa = (RSACng) certificate.PublicKey.Key)
            {
                var encryptedData = rsa.Encrypt(data, RSAEncryptionPadding.Pkcs1); // false for PKCS#1 v1.5 padding
                return encryptedData;
            }
        }
        
        public static byte[] EncryptPKCS7(string certBase64, byte[] data)
        {
            return EncryptPKCS7(ChecksumTool.ParseBase64UrlSafeString(certBase64),data);
        }
        
        public static byte[] EncryptPKCS7(byte[] cert,byte[] data)
        {
            var certificate = new X509Certificate2(cert);
        
            var contentInfo = new ContentInfo(data);
            var envelopedCms = new EnvelopedCms(contentInfo);
            var recipient = new CmsRecipient(SubjectIdentifierType.IssuerAndSerialNumber, certificate);
            envelopedCms.Encrypt(recipient);
        
            var encryptedDataPkcs7 = envelopedCms.Encode();
        
            return encryptedDataPkcs7;
        }
        
    }
}