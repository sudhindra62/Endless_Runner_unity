
using System;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using EndlessRunner.Data;

namespace EndlessRunner.Security
{
    /// <summary>
    /// Provides methods to validate the integrity of shard drops.
    /// This simulates a client-server validation process to prevent cheating.
    /// This is a core component of the AAA Rare Drop & Legendary Shard Engine.
    /// </summary>
    public static class DropIntegrityValidator
    {
        // This secret should match a secret stored on a secure server. It's used to salt the hashes.
        private const string SERVER_SECRET = "a1b2c3d4-e5f6-7g8h-9i0j-k1l2m3n4o5p6";

        /// <summary>
        /// Generates a validation hash for a specific shard drop. 
        /// In a real implementation, this would be done on the server.
        /// </summary>
        /// <param name="shardType">The type of shard being dropped.</param>
        /// <param name="salt">A unique piece of data for this transaction (e.g., timestamp, playerID).</param>
        /// <returns>A SHA256 hash string.</returns>
        public static string GenerateDropHash(ShardType shardType, string salt)
        {
            string rawData = $"{shardType.ToString()}-{salt}-{SERVER_SECRET}";
            
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array, so we convert it to a string
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        /// <summary>
        /// Validates a received drop against a provided hash.
        /// In a real implementation, the client would receive the hash from the server along with the drop.
        /// </summary>
        /// <param name="shardType">The shard that was supposedly dropped.</param>
        /// <param name="salt">The same salt used to generate the hash.</param>
        /// <param name="receivedHash">The hash provided by the "server".</param>
        /// <returns>True if the drop is valid, false otherwise.</returns>
        public static bool ValidateDrop(ShardType shardType, string salt, string receivedHash)
        {
            // The client regenerates the hash using the same data.
            string clientGeneratedHash = GenerateDropHash(shardType, salt);

            // Compare the client's hash with the server's hash.
            bool isValid = String.Equals(clientGeneratedHash, receivedHash, StringComparison.OrdinalIgnoreCase);

            if (!isValid)
            {
                Debug.LogError($"DROP_INTEGRITY_VALIDATOR: Invalid Drop Detected! Client Hash: {clientGeneratedHash}, Server Hash: {receivedHash}");
            }

            return isValid;
        }
    }
}
