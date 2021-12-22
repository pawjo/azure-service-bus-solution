using CreationApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class UserService : IUserService
    {
        private readonly DataContext _dataContext;
        private readonly IMessagingService _messagingService;
        private readonly string _databaseConnectionString;

        public UserService(DataContext dataContext, IMessagingService messagingService, IConfiguration configuration)
        {
            _dataContext = dataContext;
            _messagingService = messagingService;
            _databaseConnectionString = configuration.GetConnectionString("Default");
        }

        public async Task<bool> AddAsync(User newUser)
        {
            newUser.Active = false;
            _dataContext.Users.Add(newUser);
            var added = await _dataContext.SaveChangesAsync();

            if (added == 1)
            {
                await _messagingService.SendMessageAsync("User created " + DateTime.Now);
                return true;
            }

            return false;
        }

        public async Task<List<User>> GetActiveListAsync()
        {
            string sql = "SELECT * FROM [dbo].[User] WHERE Active = 1";
            List<User> users;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                var queryResult = await connection.QueryAsync<User>(sql);
                users = queryResult.ToList();
            }
            return users;
        }

        public async Task<User> GetByIdAsync(int id)
        {
            return await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == id);
        }

        public async Task<List<User>> GetListAsync()
        {
            string sql = "SELECT * FROM [dbo].[User]";
            List<User> users;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                var queryResult = await connection.QueryAsync<User>(sql);
                users = queryResult.ToList();
            }
            return users;
        }

        public async Task<bool> UpdateAsync(User modifiedUser)
        {
            var storedUser = await _dataContext.Users.SingleOrDefaultAsync(x => x.Id == modifiedUser.Id);

            if (storedUser == null)
            {
                return false;
            }

            storedUser.Email = modifiedUser.Email;
            storedUser.Name = modifiedUser.Name;
            storedUser.Name = modifiedUser.Name;
            storedUser.Surname = modifiedUser.Surname;
            storedUser.Active = false;

            var updated = await _dataContext.SaveChangesAsync();

            if (updated == 1)
            {
                await _messagingService.SendMessageAsync("User edited " + DateTime.Now);
                return true;
            }

            return false;
        }
    }
}
