using OddoBhf.Migrations;
using System.Security.Cryptography;

namespace OddoBhf.Helpers
{
    public class PasswordHasher
    {
        // Create an instance of a cryptographically secure random number generator.
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        // Define the size of the salt in bytes. A salt is a random value added to the password
        // before hashing to ensure that identical passwords do not produce the same hash.
        private static readonly int SaltSize = 16;

        // Define the size of the hash in bytes. This determines the length of the resulting hashed password.
        private static readonly int HashSize = 20;

        // Define the number of iterations for the key derivation function.
        // More iterations increase security but also require more computation time.
        private static readonly int Iterations = 10000;

        // This method takes a plaintext password as input and returns a securely hashed version of it.
        public static string HashPassword(string password)
        {
            // Declare a byte array to hold the salt. The salt is used to make the hash unique
            // even for identical passwords.
            byte[] salt = new byte[SaltSize];

            // Fill the salt array with cryptographically strong random bytes.
            rng.GetBytes(salt);

            // Use the PBKDF2 (Password-Based Key Derivation Function 2) to hash the password.
            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            // Generate the hashed password by extracting the specified number of bytes from the derived key.
            byte[] hash = key.GetBytes(HashSize);

            byte[] hashBytes = new byte[SaltSize + HashSize];

            // Copy the salt into the beginning of the hashBytes array.
            Array.Copy(salt, 0, hashBytes, 0, SaltSize);
            // Copy the hash into the hashBytes array, immediately following the salt.
            Array.Copy(hash, 0, hashBytes, SaltSize, HashSize);

            // Convert the combined salt and hash byte array into a Base64-encoded string.
            string base64Hash = Convert.ToBase64String(hashBytes);

            return base64Hash;
        }

        public static bool VerifyPassword(string password, string base64Hash)
        {
            var hasbBytes = Convert.FromBase64String(base64Hash);
            var salt = new byte[SaltSize];
            Array.Copy(hasbBytes, 0, salt, 0, SaltSize);

            var key = new Rfc2898DeriveBytes(password, salt, Iterations);
            byte[] hash = key.GetBytes(HashSize);

            for (var i = 0; i < HashSize; i++)
            {
                if (hasbBytes[i + SaltSize] != hash[i]) return false;
            }
            return true;
        }
    }
}