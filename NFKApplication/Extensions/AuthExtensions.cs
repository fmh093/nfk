using Konscious.Security.Cryptography;
using System.Text;

namespace NFKApplication.Extensions
{
    public static class AuthExtensions
    {
        public static string GetHashedValue(this Argon2id _, string password)
        {
            var pwBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(pwBytes);
        }
    }
}
