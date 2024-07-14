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
        Message AddMessage(Message message);
        List<Message> FindByGroupId(int groupId);
    }
}
