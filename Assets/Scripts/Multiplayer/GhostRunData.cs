using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

[System.Serializable]
public class GhostRunData
{
    public List<GhostDataPoint> dataPoints;
    public string checksum;
    public int finalScore;

    public GhostRunData(List<GhostDataPoint> points, int score)
    {
        dataPoints = points;
        finalScore = score;
        checksum = CalculateChecksum(points, score);
    }

    public bool IsValid(int theoreticalMaxScore)
    {
        if (finalScore > theoreticalMaxScore) return false;
        return checksum == CalculateChecksum(dataPoints, finalScore);
    }

    private string CalculateChecksum(List<GhostDataPoint> points, int score)
    {
        using (SHA256 sha256 = SHA256.Create())
        {
            StringBuilder sb = new StringBuilder();
            foreach (var point in points)
            {
                sb.Append(point.timestamp);
                sb.Append(point.position);
                sb.Append(point.isJumping);
                sb.Append(point.isSliding);
            }
            sb.Append(score);

            byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            return BitConverter.ToString(hash).Replace("-", "");
        }
    }

    public byte[] ToBytes()
    {
        using (MemoryStream ms = new MemoryStream())
        {
            BinaryFormatter bf = new BinaryFormatter();
            bf.Serialize(ms, this);
            return ms.ToArray();
        }
    }

    public static GhostRunData FromBytes(byte[] bytes)
    {
        using (MemoryStream ms = new MemoryStream(bytes))
        {
            BinaryFormatter bf = new BinaryFormatter();
            return (GhostRunData)bf.Deserialize(ms);
        }
    }
}
