using BusinessLayer.ResponseModels;
using DataLayer.Model;
using DataLayer.Repository;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
	public class MessageService : IMessageService
	{
        private readonly IUnitOfWork _unitOfWork;

        public MessageService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        //public Message AddMessage(MessageResponseModel message)
        //{
              
        //      _unitOfWork.Repository<MessageResponseModel>().InsertAsync(message);
        //      _unitOfWork.CommitAsync();
        //    //return message;
        //}

		public async Task<Message> AddMessage(Message message)
		{
			message.CreatedDate = DateTime.Now;
			 await _unitOfWork.Repository<Message>().InsertAsync(message);
			 await _unitOfWork.CommitAsync();
            return message;
		}

		public  async Task<List<Message>> FindByGroupId(int groupId)
        {
            return  await _unitOfWork.Repository<Message>().GetAll().Where(m => m.GroupId == groupId).ToListAsync();
        }
    }
}
