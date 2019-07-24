using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Decryptorv2
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    public class Decryptor
    {
        public static string encrypted;
        public static string decryptedData;
        public static void Main()
        {
            try
            {
                using (var random = new RNGCryptoServiceProvider())
                {
                    // Generate key
                    var key = ComputeSha256Hash("ch4s3D1bZ");
                    // Convert encrypted data (from base64)
                    byte[] byteEncrypted = Convert.FromBase64String(encrypted);
                    // Decrypt the bytes to a string.
                    decryptedData = DecryptStringFromBytes_Aes(byteEncrypted, key);

                }
            }
            catch (Exception e)
            {

            }
        }
        // Function to decrypt
        static string DecryptStringFromBytes_Aes(byte[] cipherTextCombined, byte[] Key)
        {
            string plaintext = null;
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Key;
                byte[] IV = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = new byte[cipherTextCombined.Length - IV.Length];

                Array.Copy(cipherTextCombined, IV, IV.Length);
                Array.Copy(cipherTextCombined, IV.Length, cipherText, 0, cipherText.Length);
                aesAlg.IV = IV;
                aesAlg.Mode = CipherMode.CBC;
                aesAlg.Padding = PaddingMode.PKCS7;
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
            return plaintext;
        }
        static byte[] ComputeSha256Hash(string rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return bytes;
            }
        }
    }
}


