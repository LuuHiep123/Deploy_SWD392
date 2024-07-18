using AutoMapper;
using BusinessLayer.RequestModels;
using BusinessLayer.ResponseModels;
using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
    public class NotificationService : INotificationService
    {
        private IUnitOfWork _unitOfWork;

        public NotificationService(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public async Task AddNotificationAsync(NotificationModel request, int userId)
        {
            var user = await _unitOfWork.Repository<User>().GetById(userId);
            if (user != null)
            {
                var notification = new Notification
                {
                    Content = request.Content,
                    ReceiverId = userId
                };
                await _unitOfWork.Repository<Notification>().InsertAsync(notification);
                await _unitOfWork.CommitAsync();
            }
        }
        public async Task<List<NotificationModel>> GetAllNotificationByUserId(int userId)
        {
            var listNotification = await _unitOfWork.Repository<Notification>().GetAll().Where(n => n.ReceiverId == userId).ToListAsync();

            var notificationModels = listNotification.Select(n => new NotificationModel
            {
                Content = n.Content
            }).ToList();
            return notificationModels;
        }
    }
}
