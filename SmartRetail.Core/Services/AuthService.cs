using SmartRetail.Data.Context;
using SmartRetail.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartRetail.Core.Services
{
    public class AuthService
    {
        private readonly SmartRetailDbContext _context;

        public AuthService()
        {
            _context = new SmartRetailDbContext();
        }

        public User ValidateUser(string username, string password)
        {
            var user = _context.Users
                .FirstOrDefault(u => u.Username == username && u.Password == password);

            return user; // null if invalid
        }
    }
}
