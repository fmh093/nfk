using Konscious.Security.Cryptography;
using NFKApplication.Database.Models;
using NFKApplication.Extensions;
using NFKApplication.Models;
using System.Security.Cryptography;
using System.Text;

namespace NFKApplication.Database
{
    public class AuthRepository : IAuthRepository
    {
        private readonly AppDbContext _context;

        public AuthRepository(AppDbContext context)
        {
            _context = context;
        }

        public bool TryCreateUser(string username, string password)
        {
            try
            {
                var user = new AdminUser()
                {
                    Username = username,
                    Password = HashHelper.Hash(password)
                };

                _context.AdminUsers.Add(user);
                _context.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool VerifyUser(string username, string password, out string message)
        {
            message = string.Empty;

            var user = _context.AdminUsers.FirstOrDefault(u => u.Username == username);
            if(user == null)
            {
                message = "User not found";
                return false;
            }

            var verified = HashHelper.Verify(password, user.Password);
            if(!verified)
            {
                message = "Password was incorrect";
                return false;
            }

            return true;
        }
    }

    public static class HashHelper
    {
        public static string Hash(string password)
        {
            using var argon2 = new Argon2id(Encoding.UTF8.GetBytes(password));
            argon2.Salt = SecureSalt;
            argon2.DegreeOfParallelism = 8;
            argon2.MemorySize = 65536;
            argon2.Iterations = 4;
            return argon2.GetHashedValue(password);
        }

        public static bool Verify(string password, string storedPassword)
        {
            var newHash = Hash(password);
            return newHash.SequenceEqual(storedPassword);
        }
        private static byte[] SecureSalt = new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF, 0x00, 0x11, 0x22, 0x33, 0x44, 0x55, 0x66, 0x77 };
    }


    public interface IAuthRepository
    {
        bool TryCreateUser(string username, string password);
        bool VerifyUser(string username, string password, out string message);
    }
}
