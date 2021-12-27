using CreationApp.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System;
using System.Data.SqlClient;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CreationApp.Services
{
    public class UserService : IUserService
    {
        private readonly string _databaseConnectionString;
        private readonly IReportService _reportService;

        public UserService(IConfiguration configuration, IReportService reportService)
        {
            _databaseConnectionString = configuration.GetConnectionString("Default");
            _reportService = reportService;
        }

        public async Task<bool> ActivateAsync(int userId)
        {
            string sql = "UPDATE [dbo].[User] SET Active = 1 WHERE Id = @id";

            int updated = 0;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                updated = await connection.ExecuteAsync(sql, new { id = userId });
            }

            return updated == 1;
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

        public async Task<bool> ValidateAsync(int userId)
        {
            var user = await GetByIdAsync(userId);

            if (user == null)
            {
                return false;
            }

            var result = !string.IsNullOrWhiteSpace(user.Name)
                && !string.IsNullOrWhiteSpace(user.Surname)
                && AgeIsValid(user.Age)
                && EmailIsValid(user.Email);

            var report = new Report
            {
                UserId = userId,
                Date = DateTime.Now,
                Result = result ? 2 : 1
            };
            await _reportService.AddAsync(report);

            if (result)
            {
                await ActivateAsync(userId);
            }

            return result;
        }

        private bool AgeIsValid(string age)
        {
            bool result = int.TryParse(age, out int intAge);
            if (result)
            {
                result = intAge > 0;
            }

            return age != null
                && result;
        }

        private bool EmailIsValid(string email)
        {
            var regex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            return email != null
                && regex.IsMatch(email);
        }
    }
}