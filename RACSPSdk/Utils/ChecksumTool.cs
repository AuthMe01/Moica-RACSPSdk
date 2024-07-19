using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace tw.moica.RACSPSdk.Utils
{
    public class ChecksumTool
    {
        public static string CalculateChecksum(RACSPSdkConfig config, params string[] data)
        {
            return ChecksumTool.CalculateChecksum(config.SPApiKeyBytes, data);
        }

        public static string CalculateChecksum(byte[] rsaKeyBytes, params string[] data)
        {
            var nuullToEmpty = data.Select(n => n ?? "");
            string calcCheckSum = GetSHA256inHex(string.Join("", nuullToEmpty));
            return EncryptWithAesGcmRandomIvInBytes(rsaKeyBytes, calcCheckSum);
        }

        public static string GetSHA256inHex(string baseString)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(baseString);
                    byte[] salt = Encoding.UTF8.GetBytes("");
                    byte[] all = new byte[passwordBytes.Length + salt.Length];
                    Buffer.BlockCopy(passwordBytes, 0, all, 0, passwordBytes.Length);
                    Buffer.BlockCopy(salt, 0, all, passwordBytes.Length, salt.Length);

                    byte[] hash = sha256.ComputeHash(all);
                    return ConvertBinaryToHexString(hash);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error computing SHA256 hash", ex);
            }
        }
        
        public static string GetSHA256inBase64UrlSafe(string baseString)
        {
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] passwordBytes = Encoding.UTF8.GetBytes(baseString);
                    byte[] salt = Encoding.UTF8.GetBytes("");
                    byte[] all = new byte[passwordBytes.Length + salt.Length];
                    Buffer.BlockCopy(passwordBytes, 0, all, 0, passwordBytes.Length);
                    Buffer.BlockCopy(salt, 0, all, passwordBytes.Length, salt.Length);

                    byte[] hash = sha256.ComputeHash(all);
                    return ToBase64UrlSafeString(hash);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error computing SHA256 hash", ex);
            }
        }

        public static string EncryptWithAesGcmRandomIvInBytes(byte[] aesKey, string plainString)
        {
            try
            {
                byte[] iv = new byte[12]; // GCM IV length
                using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
                {
                    rng.GetBytes(iv);
                }

                using (AesGcm aesGcm = new AesGcm(aesKey))
                {
                    byte[] plainText = Encoding.UTF8.GetBytes(plainString);
                    byte[] cipherText = new byte[plainText.Length];
                    byte[] tag = new byte[16]; // GCM tag length

                    aesGcm.Encrypt(iv, plainText, cipherText, tag);

                    byte[] result = new byte[iv.Length + cipherText.Length + tag.Length];
                    Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                    Buffer.BlockCopy(cipherText, 0, result, iv.Length, cipherText.Length);
                    Buffer.BlockCopy(tag, 0, result, iv.Length + cipherText.Length, tag.Length);

                    return ConvertBinaryToHexString(result);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error encrypting with AES-GCM", ex);
            }
        }
        
        public static string ToBase64UrlSafeString(byte[] data)
        {
            // Convert to Base64 string
            string base64 = Convert.ToBase64String(data);

            // Make it URL-safe
            base64 = base64.Replace('+', '-').Replace('/', '_');

            // Remove padding characters
            base64 = base64.TrimEnd('=');

            return base64;
        }

        public static byte[] ParseBase64UrlSafeString(string base64UrlSafeString)
        {
            // Replace URL-safe characters with standard Base64 characters
            string base64 = base64UrlSafeString.Replace('-', '+').Replace('_', '/');

            // Add padding if necessary
            switch (base64.Length % 4)
            {
                case 2:
                    base64 += "==";
                    break;
                case 3:
                    base64 += "=";
                    break;
            }

            // Decode the Base64 string
            return Convert.FromBase64String(base64);
        }

        public static string DecryptWithAesGcmRandomIvInBytes(RACSPSdkConfig config, string checksum)
        {
            return DecryptWithAesGcmRandomIvInBytes(config.SPApiKeyBytes, checksum);
        }

        public static string DecryptWithAesGcmRandomIvInBytes(byte[] aesKey, string checksum)
        {
            try
            {
                byte[] cipherVal = ConvertHexStringToBinary(checksum);

                byte[] iv = new byte[12]; // GCM IV length
                Buffer.BlockCopy(cipherVal, 0, iv, 0, iv.Length);

                using (AesGcm aesGcm = new AesGcm(aesKey))
                {
                    byte[] cipherText = new byte[cipherVal.Length - iv.Length - 16]; // 16 bytes for the GCM tag
                    byte[] tag = new byte[16];

                    Buffer.BlockCopy(cipherVal, iv.Length, cipherText, 0, cipherText.Length);
                    Buffer.BlockCopy(cipherVal, iv.Length + cipherText.Length, tag, 0, tag.Length);

                    byte[] plainText = new byte[cipherText.Length];

                    aesGcm.Decrypt(iv, cipherText, tag, plainText);

                    return Encoding.UTF8.GetString(plainText);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error decrypting with AES-GCM", ex);
            }
        }


        private static byte[] ConvertHexStringToBinary(string hexString)
        {
            int numberChars = hexString.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexString.Substring(i, 2), 16);
            }

            return bytes;
        }


        public static string ConvertBinaryToHexString(byte[] data)
        {
            StringBuilder hexString = new StringBuilder(data.Length * 2);
            foreach (byte b in data)
            {
                hexString.AppendFormat("{0:x2}", b);
            }

            return hexString.ToString();
        }
    }
}