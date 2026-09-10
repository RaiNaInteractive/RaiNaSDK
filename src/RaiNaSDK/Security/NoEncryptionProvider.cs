namespace RaiNa.Security
{
    public class NoEncryptionProvider : IEncryptionProvider
    {
        public byte[] Decrypt(byte[] ciphertext) => ciphertext;
        public byte[] Encrypt(byte[] plaintext) => plaintext;
    }
}
