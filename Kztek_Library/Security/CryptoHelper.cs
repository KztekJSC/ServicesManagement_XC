using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Kztek.Security;

namespace Kztek_Library.Security
{
    public class CryptoHelper
    {
        /// <summary>
        /// Mã hóa mật khẩu người dùng
        /// </summary>
        /// <param name="pass">mk gốc</param>
        /// <param name="passsalat">mk salat của mỗi tk</param>
        /// <returns>mk mã hóa</returns>
        public static string EncryptPass_User(string pass, string passsalat)
        {   
            var privatekey = SecurityModel.System_Key;

            //Mã hóa lần 1
            pass = CryptoProvider.SimpleEncryptWithPassword(pass, passsalat);

            //Mã hóa lần 2
            pass = CryptoProvider.SimpleEncryptWithPassword(pass, privatekey);

            return pass;
        }

        /// <summary>
        /// Giải mã mật khẩu
        /// </summary>
        /// <param name="pass">mk mã hóa</param>
        /// <param name="passsalat">mk salat của mỗi tk</param>
        /// <returns>mk gốc</returns>
        public static string DecryptPass_User(string pass, string passsalat)
        {   
            var privatekey = SecurityModel.System_Key;

            //Giải mã lần 1
            pass = CryptoProvider.SimpleDecryptWithPassword(pass, privatekey);

            //Giải mã lần 2
            pass = CryptoProvider.SimpleDecryptWithPassword(pass, passsalat);

            return pass;
        }

        /// <summary>
        /// Mã hóa thông tin session người dùng đăng nhập
        /// </summary>
        /// <param name="userjson">Serialize model gốc</param>
        /// <returns>thông tin mã hóa để lưu</returns>
        public static string EncryptSessionCookie_User(string userjson)
        {   
            var privatekey = SecurityModel.Session_Key;

            //Mã hóa lần 1
            userjson = CryptoProvider.SimpleEncryptWithPassword(userjson, privatekey);

            //Mã hóa lần 2
            return userjson;
        }

        /// <summary>
        /// Giải mã thông tin session người dùng đăng nhập
        /// </summary>
        /// <param name="userjson">model đã mã hóa</param>
        /// <returns>Serialize model gốc</returns>
        public static string DecryptSessionCookie_User(string userjson)
        {   
            var privatekey = SecurityModel.Session_Key;

            //Mã hóa lần 1
            userjson = CryptoProvider.SimpleDecryptWithPassword(userjson, privatekey);

            //Mã hóa lần 2
            return userjson;
        }

        /// <summary>
        /// Mã hóa thông tin session người dùng đăng nhập
        /// </summary>
        /// <param name="pass">Serialize pass</param>
        /// <returns>thông tin mã hóa để lưu</returns>
        public static string EncryptFtpPass(string pass)
        {
            var privatekey = SecurityModel.Session_Key;

            //Mã hóa lần 1
            pass = CryptoProvider.SimpleEncryptWithPassword(pass, privatekey);

            //Mã hóa lần 2
            return pass;
        }

        /// <summary>
        /// Giải mã thông tin session người dùng đăng nhập
        /// </summary>
        /// <param name="pass">pass đã mã hóa</param>
        /// <returns>Serialize model gốc</returns>
        public static string DecryptFtpPass(string pass)
        {
            var privatekey = SecurityModel.Session_Key;

            //Mã hóa lần 1
            pass = CryptoProvider.SimpleDecryptWithPassword(pass, privatekey);

            //Mã hóa lần 2
            return pass;
        }

        /// <summary>
        /// Encrypt a string using dual encryption method. Return a encrypted cipher Text
        /// </summary>
        /// <param name="toEncrypt">string to be encrypted</param>
        /// <param name="useHashing">use hashing? send to for extra secirity</param>
        /// <returns></returns>
        public static string Encrypt(string toEncrypt, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Encoding.UTF8.GetBytes(toEncrypt);

            string key = "17032008";
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
            tdes.Clear();
            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }
        /// <summary>
        /// DeCrypt a string using dual encryption method. Return a DeCrypted clear string
        /// </summary>
        /// <param name="cipherString">encrypted string</param>
        /// <param name="useHashing">Did you use hashing to encrypt this data? pass true is yes</param>
        /// <returns></returns>
        public static string Decrypt(string cipherString, bool useHashing)
        {
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(cipherString);

            string key = "17032008";
            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(Encoding.UTF8.GetBytes(key));
                hashmd5.Clear();
            }
            else
                keyArray = Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            tdes.Clear();
            return Encoding.UTF8.GetString(resultArray);
        }

        public static string OpenSSLEncrypt(string plainText, string passphrase)
        {
            // generate salt
            byte[] key, iv;
            byte[] salt = new byte[8];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(salt);
            DeriveKeyAndIV(passphrase, salt, out key, out iv);
            // encrypt bytes
            byte[] encryptedBytes = EncryptStringToBytesAes(plainText, key, iv);
            // add salt as first 8 bytes
            byte[] encryptedBytesWithSalt = new byte[salt.Length + encryptedBytes.Length + 8];
            Buffer.BlockCopy(Encoding.ASCII.GetBytes("Salted__"), 0, encryptedBytesWithSalt, 0, 8);
            Buffer.BlockCopy(salt, 0, encryptedBytesWithSalt, 8, salt.Length);
            Buffer.BlockCopy(encryptedBytes, 0, encryptedBytesWithSalt, salt.Length + 8, encryptedBytes.Length);
            // base64 encode
            return Convert.ToBase64String(encryptedBytesWithSalt);
        }

        public static string OpenSSLDecrypt(string encrypted, string passphrase)
        {
            // base 64 decode
            byte[] encryptedBytesWithSalt = Convert.FromBase64String(encrypted);
            // extract salt (first 8 bytes of encrypted)
            byte[] salt = new byte[8];
            byte[] encryptedBytes = new byte[encryptedBytesWithSalt.Length - salt.Length - 8];
            Buffer.BlockCopy(encryptedBytesWithSalt, 8, salt, 0, salt.Length);
            Buffer.BlockCopy(encryptedBytesWithSalt, salt.Length + 8, encryptedBytes, 0, encryptedBytes.Length);
            // get key and iv
            byte[] key, iv;
            DeriveKeyAndIV(passphrase, salt, out key, out iv);
            return DecryptStringFromBytesAes(encryptedBytes, key, iv);
        }

        private static void DeriveKeyAndIV(string passphrase, byte[] salt, out byte[] key, out byte[] iv)
        {
            // generate key and iv
            List<byte> concatenatedHashes = new List<byte>(48);

            byte[] password = Encoding.UTF8.GetBytes(passphrase);
            byte[] currentHash = new byte[0];
            MD5 md5 = MD5.Create();
            bool enoughBytesForKey = false;
            // See http://www.openssl.org/docs/crypto/EVP_BytesToKey.html#KEY_DERIVATION_ALGORITHM
            while (!enoughBytesForKey)
            {
                int preHashLength = currentHash.Length + password.Length + salt.Length;
                byte[] preHash = new byte[preHashLength];

                Buffer.BlockCopy(currentHash, 0, preHash, 0, currentHash.Length);
                Buffer.BlockCopy(password, 0, preHash, currentHash.Length, password.Length);
                Buffer.BlockCopy(salt, 0, preHash, currentHash.Length + password.Length, salt.Length);

                currentHash = md5.ComputeHash(preHash);
                concatenatedHashes.AddRange(currentHash);

                if (concatenatedHashes.Count >= 48)
                    enoughBytesForKey = true;
            }

            key = new byte[32];
            iv = new byte[16];
            concatenatedHashes.CopyTo(0, key, 0, 32);
            concatenatedHashes.CopyTo(32, iv, 0, 16);

            md5.Clear();
            md5 = null;
        }

        static byte[] EncryptStringToBytesAes(string plainText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the stream used to encrypt to an in memory
            // array of bytes.
            MemoryStream msEncrypt;

            // Declare the RijndaelManaged object
            // used to encrypt the data.
            RijndaelManaged aesAlg = null;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption.
                msEncrypt = new MemoryStream();
                using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                {
                    using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                    {

                        //Write all data to the stream.
                        swEncrypt.Write(plainText);
                        swEncrypt.Flush();
                        swEncrypt.Close();
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            // Return the encrypted bytes from the memory stream.
            return msEncrypt.ToArray();
        }

        static string DecryptStringFromBytesAes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (key == null || key.Length <= 0)
                throw new ArgumentNullException("key");
            if (iv == null || iv.Length <= 0)
                throw new ArgumentNullException("iv");

            // Declare the RijndaelManaged object
            // used to decrypt the data.
            RijndaelManaged aesAlg = null;

            // Declare the string used to hold
            // the decrypted text.
            string plaintext;

            try
            {
                // Create a RijndaelManaged object
                // with the specified key and IV.
                aesAlg = new RijndaelManaged { Mode = CipherMode.CBC, KeySize = 256, BlockSize = 128, Key = key, IV = iv };

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                            srDecrypt.Close();
                        }
                    }
                }
            }
            finally
            {
                // Clear the RijndaelManaged object.
                if (aesAlg != null)
                    aesAlg.Clear();
            }

            return plaintext;
        }

        /// <summary>
        /// Mã hóa mật khẩu admin
        /// </summary>
        /// <param name="pass">mk gốc</param>
        /// <returns>mk mã hóa</returns>
        public static string EncryptPass_Admin(string pass)
        {
            var privatekey = SecurityModel.Admin_Key;

            //Mã hóa lần 1
            pass = CryptoProvider.SimpleEncryptWithPassword(pass, privatekey);

            return pass;
        }

        /// <summary>
        /// Giải mã mật khẩu admin
        /// </summary>
        /// <param name="pass">mk mã hóa</param>
        /// <returns>mk gốc</returns>
        public static string DecryptPass_Admin(string pass)
        {
            var privatekey = SecurityModel.Admin_Key;

            //Giải mã lần 2
            pass = CryptoProvider.SimpleDecryptWithPassword(pass, privatekey);

            return pass;
        }

        /// <summary>
        /// Mã hóa thông tin session admin
        /// </summary>
        /// <param name="userjson">Serialize model gốc</param>
        /// <returns>thông tin mã hóa để lưu</returns>
        public static string EncryptSessionCookie_Admin(string userjson)
        {
            var privatekey = SecurityModel.Admin_Key;

            //Mã hóa lần 1
            userjson = CryptoProvider.SimpleEncryptWithPassword(userjson, privatekey);

            //Mã hóa lần 2
            return userjson;
        }

        /// <summary>
        /// Giải mã thông tin session admin
        /// </summary>
        /// <param name="userjson">model đã mã hóa</param>
        /// <returns>Serialize model gốc</returns>
        public static string DecryptSessionCookie_Admin(string userjson)
        {
            var privatekey = SecurityModel.Admin_Key;

            //Mã hóa lần 1
            userjson = CryptoProvider.SimpleDecryptWithPassword(userjson, privatekey);

            //Mã hóa lần 2
            return userjson;
        }
    }
}