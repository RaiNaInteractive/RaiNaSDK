using System.IO;
using System.Text;
using System.Security.Cryptography;

namespace RaiNa.Security
{
    public sealed class AesEncryptionProvider
    {
        private readonly byte[] _key;

        public AesEncryptionProvider(string key)
        {
            using var sha = SHA256.Create();
            _key = sha.ComputeHash(Encoding.UTF8.GetBytes(key));
        }

        public byte[] Encrypt(byte[] plaintext)
        {
            using var aes = Aes.Create();
            aes.Key     = _key;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            byte[] ciphertext   = encryptor.TransformFinalBlock(plaintext, 0, plaintext.Length);

            using var ms = new MemoryStream();
            using var bw = new BinaryWriter(ms);
            bw.Write(aes.IV.Length);
            bw.Write(aes.IV);
            bw.Write(ciphertext);
            return ms.ToArray();
        }

        public byte[] Decrypt(byte[] ciphertext)
        {
            using var ms = new MemoryStream(ciphertext);
            using var br = new BinaryReader(ms);

            int    ivLen = br.ReadInt32();
            byte[] iv    = br.ReadBytes(ivLen);
            byte[] cipher = br.ReadBytes((int)(ms.Length - ms.Position));

            using var aes = Aes.Create();
            aes.Key     = _key;
            aes.IV      = iv;
            aes.Mode    = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;

            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        }
    }
}
