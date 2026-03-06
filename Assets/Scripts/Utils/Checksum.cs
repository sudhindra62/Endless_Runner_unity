using System.Security.Cryptography;
using System.Text;

public static class Checksum
{
    public static string Calculate(string data)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(data));
            return System.BitConverter.ToString(hash).Replace("-", "").ToLower();
        }
    }
}
