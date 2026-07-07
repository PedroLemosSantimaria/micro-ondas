using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Microondas.Core.Interfaces;
using Microondas.Core.Models;

namespace Microondas.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string usersFilePath;

        public UserRepository()
        {
            var dataFolder = Path.Combine(System.AppContext.BaseDirectory, "Data");

            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }

            usersFilePath = Path.Combine(dataFolder, "users.json");

            if (!File.Exists(usersFilePath))
            {
                File.WriteAllText(usersFilePath, "[]");
            }
        }

        public AuthUser GetByUserName(string userName)
        {
            var users = GetUsers();
            return users.FirstOrDefault(x => x.UserName.ToLower() == userName.ToLower());
        }

        private List<AuthUser> GetUsers()
        {
            if (!File.Exists(usersFilePath))
                return new List<AuthUser>();

            var json = File.ReadAllText(usersFilePath);

            if (string.IsNullOrWhiteSpace(json))
                return new List<AuthUser>();

            var users = JsonSerializer.Deserialize<List<AuthUser>>(json);
            return users ?? new List<AuthUser>();
        }
    }
}