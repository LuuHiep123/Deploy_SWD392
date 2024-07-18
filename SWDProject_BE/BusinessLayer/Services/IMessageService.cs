using BusinessLayer.ResponseModels;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IMessageService
    {
        Task<Message> AddMessage(Message message);
        Task<List<Message>> FindByGroupId(int groupId);
    }
}
