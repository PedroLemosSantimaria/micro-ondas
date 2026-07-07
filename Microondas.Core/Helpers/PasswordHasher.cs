using System.Security.Cryptography;
using System.Text;

namespace Microondas.Core.Helpers
{
    public static class PasswordHasher
    {
        public static string ToSha256(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                return string.Empty;

            using var sha = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(text);
            var hash = sha.ComputeHash(bytes);

            var sb = new StringBuilder();

            foreach (var item in hash)
            {
                sb.Append(item.ToString("x2"));
            }

            return sb.ToString();
        }
    }
}