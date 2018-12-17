using System;
using System.IO;
using System.Security.Cryptography;

namespace SyCoin.Helpers
{
    public static class DigitalSignatureHelper
    {
        public static byte[] HashAndSignBytes(Stream DataStream, RSAParameters Key)
        {

            try
            {
                // Reset the current position in the stream to 
                // the beginning of the stream (0). RSACryptoServiceProvider
                // can't verify the data unless the stream position
                // is set to the starting position of the data.
                DataStream.Position = 0;

                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.  
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Hash and sign the data. Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.SignData(DataStream, new SHA256CryptoServiceProvider());
            }
            catch
            {
                return null;
            }
        }

        public static bool VerifySignedHashBytes(byte[] DataToVerify, byte[] SignedData, RSAParameters Key)
        {
            try
            {
                // Create a new instance of RSACryptoServiceProvider using the 
                // key from RSAParameters.
                RSACryptoServiceProvider RSAalg = new RSACryptoServiceProvider();

                RSAalg.ImportParameters(Key);

                // Verify the data using the signature.  Pass a new instance of SHA1CryptoServiceProvider
                // to specify the use of SHA1 for hashing.
                return RSAalg.VerifyData(DataToVerify, new SHA256CryptoServiceProvider(), SignedData);

            }
            catch
            {
                return false;
            }
        }

        public static byte[] SignHash(byte[] hash, RSAParameters key)
        {
            RSACryptoServiceProvider rSACrypto = new RSACryptoServiceProvider();
            rSACrypto.ImportParameters(key);

            return rSACrypto.SignHash(hash, "SHA256");
        }

        public static bool VerifySignedHash(byte[] hash, byte[] signature , RSAParameters key)
        {
            RSACryptoServiceProvider rSACrypto = new RSACryptoServiceProvider();
            rSACrypto.ImportParameters(key);
            return rSACrypto.VerifyHash(hash, "SHA256", signature);
        }

    }
}
