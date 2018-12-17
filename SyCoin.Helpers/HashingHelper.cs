using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SyCoin.Helpers
{
    public static class HashingHelper
    {
        public static byte[] HashObject(object obj)
        {
            string objInJson = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(objInJson);
            return SHA256.Create().ComputeHash(data);
        }

        public static string ByteArrayToHexDigit(byte[] data) => BitConverter.ToString(data).Replace("-", "");

        public static bool IsHashMeetTarget(string hash, uint target)
        {
            for (int i = 0; i <= target; i++)
                if (hash[i] != '0') return false;

            return true;
        }
    }
}
