using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

/// <summary>
/// Validates the integrity of drops and runs to prevent cheating.
/// Global scope.
/// </summary>
public class DropIntegrityValidator : Singleton<DropIntegrityValidator>
{
    private const string SERVER_SECRET = "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6";

    public bool IsRunValid(float score, float duration, int revives)
    {
        if (duration > 0 && (score / duration) > 10000) return false;
        if (revives > 10) return false;
        return true;
    }

    public static string GenerateDropHash(string shardType, string salt)
    {
        string rawData = $"{shardType}-{salt}-{SERVER_SECRET}";
        using (SHA256 sha256Hash = SHA256.Create())
        {
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++) builder.Append(bytes[i].ToString("x2"));
            return builder.ToString();
        }
    }

    public static bool ValidateDrop(string shardType, string salt, string receivedHash)
    {
        string clientGeneratedHash = GenerateDropHash(shardType, salt);
        return string.Equals(clientGeneratedHash, receivedHash, StringComparison.OrdinalIgnoreCase);
    }
}
