using BusinessLayer.RequestModels.Report;
using BusinessLayer.ResponseModels.Report;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IReportService
    {
        Task<List<ReportResponseModel>> GetAll();
        Task<List<ReportResponseModel>> GetAllValidReport();
        Task<List<ReportResponseModel>> GetReportByUserId(int userId);
        Task<List<ReportResponseModel>> GetReportByPostId(int postId);
        Task<string> AddReportByUser(ReportRequestaUser dto, int userId);
        Task<string> UpdateReportByUser(int id,ReportRequestaUser dto);
        Task<string> DeleteReport(int id);
        Task<string> AcceptReport(int id);
    }
}
