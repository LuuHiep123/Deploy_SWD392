using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
	public interface IGroupService
	{
		Task AddGroupAsync(Group group);
		Task<List<Group>> FindAllByUserId(int userId);

    }
}
