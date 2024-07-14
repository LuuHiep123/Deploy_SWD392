using DataLayer.Model;
using DataLayer.Repository;
using DataLayer.UnitOfWork;
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

        public Message AddMessage(Message message)
        {
              message.CreatedDate = DateTime.Now;
             _unitOfWork.Repository<Message>().InsertAsync(message);
             _unitOfWork.CommitAsync();
            return message;
        }

        public  List<Message> FindByGroupId(int groupId)
        {
            return  _unitOfWork.Repository<Message>().GetAll().Where(m => m.GroupId == groupId).ToList();
        }
    }
}
