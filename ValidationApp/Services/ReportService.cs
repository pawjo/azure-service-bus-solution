using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using ValidationApp.ViewModels;

namespace CreationApp.Services
{
    public class ReportService : IReportService
    {
        private readonly string _databaseConnectionString;

        public ReportService(IConfiguration configuration)
        {
            _databaseConnectionString = configuration.GetConnectionString("Default");
        }

        public async Task<ReportListViewModel> GetList()
        {
            var sql = "SELECT r.Id, u.Email, r.Date, r.Result FROM [dbo].[Report] r JOIN [dbo].[User] u ON r.UserId = u.Id";

            var reportListViewModel = new ReportListViewModel();
            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                var queryResult = await connection.QueryAsync<dynamic>(sql);

                reportListViewModel.Reports = new List<ReportListItem>();

                foreach (var item in queryResult)
                {
                    var report = new ReportListItem
                    {
                        Id = item.Id,
                        UserEmail = item.UserEmail,
                        Date = item.Date,
                        Result = item.Result
                    };

                    reportListViewModel.Reports.Add(report);
                }
            }

            return reportListViewModel;
        }
    }
}
