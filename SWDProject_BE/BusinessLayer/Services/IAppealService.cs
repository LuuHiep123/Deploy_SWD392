using BusinessLayer.RequestModels.Appeal;
using BusinessLayer.ResponseModels.Appeal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IAppealService
    {
        Task<List<AppealResponseModel>> GetAll();
        Task<AppealResponseModel> FindAppealById(int AppealId);
        Task<List<AppealResponseModel>> GetAllAppealProcessingll();
        Task<List<AppealResponseModel>> GetAllByUserId(int userId);
        Task<List<AppealResponseModel>> GetAllByBannerAccountId(int BannerAccountId);
        Task<string> AcceptAppeal(int AppealId);
        Task<string> UpdateAppeal(UppdateAppealRequestModel dto, int appealId);
        Task<string> AddAppeal(AddAppealRequestModel dto, int Userid);
    }
}
