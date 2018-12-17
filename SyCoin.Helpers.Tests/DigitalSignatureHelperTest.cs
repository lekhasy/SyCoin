using System;
using Xunit;
using System.Security.Cryptography;

namespace SyCoin.Helpers.Tests
{
    public class DigitalSignatureHelperTest
    {
        [Fact]
        public void SignHashTest()
        {
            var a = 1056;

            var hashOfa = HashingHelper.HashObject(a);

            var rsa = RSA.Create();

            var key = rsa.ExportParameters(true);

            byte[] signature = DigitalSignatureHelper.SignHash(hashOfa, key);

            var ok = DigitalSignatureHelper.VerifySignedHash(hashOfa, signature, key);

            Assert.True(ok);
        }

        [Fact]
        public void VerifyWrongHashTest()
        {
            var a = 1056;

            var b = 1057;

            var hashOfa = HashingHelper.HashObject(a);

            var hashOfb = HashingHelper.HashObject(b);

            var rsa = RSA.Create();

            var key = rsa.ExportParameters(true);

            byte[] signature = DigitalSignatureHelper.SignHash(hashOfa, key);

            var ok = DigitalSignatureHelper.VerifySignedHash(hashOfb, signature, key);

            Assert.False(ok);
        }
    }
}
