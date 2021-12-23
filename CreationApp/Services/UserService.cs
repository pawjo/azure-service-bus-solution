using CreationApp.Models;
using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class UserService : IUserService
    {
        private readonly string _databaseConnectionString;
        private readonly IMessagingService _messagingService;

        public UserService(IConfiguration configuration, IMessagingService messagingService)
        {
            _databaseConnectionString = configuration.GetConnectionString("Default");
            _messagingService = messagingService;
        }

        public async Task<bool> AddAsync(User newUser)
        {
            // Adding dynamic parameters for not taking unnecessary properties and add output parameter
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Email", newUser.Email);
            dynamicParameters.Add("Name", newUser.Name);
            dynamicParameters.Add("Surname", newUser.Surname);
            dynamicParameters.Add("Age", newUser.Age);
            dynamicParameters.Add("NewUserId", dbType: DbType.Int32, direction: ParameterDirection.Output);

            int added = 0;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                added = await connection.ExecuteAsync("CreateNewUser", dynamicParameters, commandType: CommandType.StoredProcedure);
            }

            var newUserId = dynamicParameters.Get<dynamic>("NewUserId");
            if (added == 1 && newUserId > 0)
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
            string sql = "SELECT * FROM [dbo].[User] WHERE Id = @UserId";
            User user;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                // If there is more than one elements method should return null
                try
                {
                    user = await connection.QuerySingleOrDefaultAsync<User>(sql, new { UserId = id });
                }
                catch
                {
                    user = null;
                }
            }
            return user;
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
            // Adding dynamic parameters for not taking unnecessary properties
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("Id", modifiedUser.Id);
            dynamicParameters.Add("Email", modifiedUser.Email);
            dynamicParameters.Add("Name", modifiedUser.Name);
            dynamicParameters.Add("Surname", modifiedUser.Surname);
            dynamicParameters.Add("Age", modifiedUser.Age);

            int updated = 0;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                updated = await connection.ExecuteAsync("UpdateUser", dynamicParameters, commandType: CommandType.StoredProcedure);
            }

            if (updated == 1)
            {
                await _messagingService.SendMessageAsync("User edited " + DateTime.Now);
                return true;
            }

            return false;
        }
    }
}
