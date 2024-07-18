using BusinessLayer.RequestModels;
using BusinessLayer.ResponseModels;
using BusinessLayer.ResponseModels.Order;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface INotificationService
    {
        Task<List<NotificationModel>> GetAllNotificationByUserId(int userId);

        Task AddNotificationAsync(NotificationModel notification, int userId);
    }
}
