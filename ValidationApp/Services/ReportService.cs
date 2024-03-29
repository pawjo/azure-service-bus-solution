﻿using CreationApp.Models;
using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
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

        public async Task<bool> AddAsync(Report report)
        {
            var dynamicParameters = new DynamicParameters();
            dynamicParameters.Add("UserId", report.UserId);
            dynamicParameters.Add("Date", report.Date);
            dynamicParameters.Add("Result", report.Result);

            int added = 0;

            using (var connection = new SqlConnection(_databaseConnectionString))
            {
                added = await connection.ExecuteAsync("CreateNewReport", dynamicParameters, commandType: CommandType.StoredProcedure);
            }

            return added == 1;
        }

        public async Task<ReportListViewModel> GetListAsync()
        {
            var sql = "SELECT TOP 10 r.Id, u.Email, r.Date, r.Result FROM [dbo].[Report] r JOIN [dbo].[User] u ON r.UserId = u.Id ORDER BY r.Id DESC";

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
                        UserEmail = item.Email,
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
