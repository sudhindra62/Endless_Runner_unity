using UnityEngine;

public class SaveIntegrityGuard
{
    public string GenerateChecksum(string data)
    {
        return "checksum_" + data.GetHashCode().ToString();
    }

    public bool IsChecksumValid(string data, string checksum)
    {
        return checksum == GenerateChecksum(data);
    }
}
