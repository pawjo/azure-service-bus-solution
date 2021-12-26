using System.Threading.Tasks;
using ValidationApp.ViewModels;

namespace CreationApp.Services
{
    public interface IReportService
    {
        public Task<ReportListViewModel> GetList();
    }
}
