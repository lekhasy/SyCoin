using System;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace SyCoin.Helpers
{
    public static class HashingHelper
    {
        public static string HashObject(object obj)
        {
            string objInJson = JsonConvert.SerializeObject(obj);
            var data = Encoding.UTF8.GetBytes(objInJson);
            var hashBytes = SHA256.Create().ComputeHash(data);
            return BitConverter.ToString(hashBytes).Replace("-", "");
        }

        public static bool IsHashMeetTarget(string hash, uint target)
        {
            for (int i = 0; i <= target; i++)
                if (hash[i] != '0') return false;

            return true;
        }
    }
}
