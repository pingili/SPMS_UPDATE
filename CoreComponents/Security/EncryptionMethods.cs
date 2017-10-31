using System;
using System.Web;
using System.Configuration;
using System.Collections;
using System.Text;
using System.Security.Cryptography;
using System.IO;
using System.Xml;

namespace CoreComponents.Security
{
    public class EncryptionMethods
    {

        /*public static string EncryptString(string strQueryString)
        {
            return Encrypt(strQueryString, "!#$a54?3");
        }

        public static string DecryptString(string strQueryString)
        {
            return Decrypt(strQueryString, "!#$a54?3");
        }

        private static string Decrypt(string stringToDecrypt, string sEncryptionKey)
        {

            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            stringToDecrypt = stringToDecrypt.Replace(" ", "+");

            byte[] inputByteArray = new byte[stringToDecrypt.Length];
            try
            {

                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Convert.FromBase64String(stringToDecrypt);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                Encoding encoding = Encoding.UTF8; return encoding.GetString(ms.ToArray());
            }

            catch (System.Exception ex)
            {

                throw ex;
            }

        }

        private static string Encrypt(string stringToEncrypt, string sEncryptionKey)
        {

            byte[] key = { };
            byte[] IV = { 10, 20, 30, 40, 50, 60, 70, 80 };

            byte[] inputByteArray; //Convert.ToByte(stringToEncrypt.Length)

            try
            {

                key = Encoding.UTF8.GetBytes(sEncryptionKey.Substring(0, 8));
                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                inputByteArray = Encoding.UTF8.GetBytes(stringToEncrypt);
                MemoryStream ms = new MemoryStream();

                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, IV), CryptoStreamMode.Write);
                cs.Write(inputByteArray, 0, inputByteArray.Length);

                cs.FlushFinalBlock();

                return Convert.ToBase64String(ms.ToArray()).Replace("+", " ");
            }

            catch (System.Exception ex)
            {

                throw ex;
            }

        }//end of Encrypt
       */

        public static string EncryptString(string strQueryString)
        {
            return ConvertStringToHex(strQueryString, System.Text.Encoding.Unicode);
        }

        public static string DecryptString(string strQueryString)
        {
            return ConvertHexToString(strQueryString, System.Text.Encoding.Unicode);
        }

        public static string ConvertStringToHex(String input, System.Text.Encoding encoding)
        {
            Byte[] stringBytes = encoding.GetBytes(input);
            StringBuilder sbBytes = new StringBuilder(stringBytes.Length * 2);
            foreach (byte b in stringBytes)
            {
                sbBytes.AppendFormat("{0:X2}", b);
            }
            return sbBytes.ToString();
        }

        public static string ConvertHexToString(String hexInput, System.Text.Encoding encoding)
        {
            int numberChars = hexInput.Length;
            byte[] bytes = new byte[numberChars / 2];
            for (int i = 0; i < numberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(hexInput.Substring(i, 2), 16);
            }
            return encoding.GetString(bytes);
        }
    }
}