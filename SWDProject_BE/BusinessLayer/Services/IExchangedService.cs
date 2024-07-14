using BusinessLayer.RequestModels;
using BusinessLayer.ResponseModels;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IExchangedService
    {
        Task AddExchangedAsync(Exchanged exchanged);
        Task AddExchangedProductAsync(ExchangedProduct exchangedProduct);
        Task<IEnumerable<ExchangedResponseModel>> GetAllFinishedExchangedByUserIdAsync(int userId);
        Task<IEnumerable<ExchangedResponseModel>> GetAllPendingExchangedByUserIdForCustomerAsync(int userId);
        Task<IEnumerable<ExchangedResponseModel>> GetAllPendingExchangedByUserIdForPosterAsync(int userId);
        Task<Exchanged> GetExchangedByIdAsync(int id);
        Task UpdateExchangedStatusAcceptAsync(int id);
        Task UpdateExchangedStatusDenyAsync(int id);
        Task CancelExchaneAsync(int id);
        Task DeleteExchangedAsync(int id);
    }
}
