namespace RaiNa.Security
{
    public interface IEncryptionProvider
    {
        byte[] Encrypt(byte[] plaintext);
        byte[] Decrypt(byte[] ciphertext);
    }
}
