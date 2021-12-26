using CreationApp.Models;
using System.Threading.Tasks;
using ValidationApp.ViewModels;

namespace CreationApp.Services
{
    public interface IReportService
    {
        public Task<bool> AddAsync(Report report);

        public Task<ReportListViewModel> GetListAsync();
    }
}
