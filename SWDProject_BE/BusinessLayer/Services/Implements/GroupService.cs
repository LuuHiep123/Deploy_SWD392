using DataLayer.Model;
using DataLayer.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
	public class GroupService : IGroupService
	{
        private readonly IUnitOfWork _unitOfWork;

        public GroupService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddGroupAsync(Group group)
		{
            group.CreatedDate = DateTime.Now;

            await _unitOfWork.Repository<Group>().InsertAsync(group);
            await _unitOfWork.CommitAsync();
        }

		public List<Group> FindAllByUserId(int userId)
		{
			var user = _unitOfWork.Repository<User>().Find(u => u.Id == userId);
			if (user == null)
			{
				return null;
			}
			return _unitOfWork.Repository<Group>().GetAll().Where(g => g.UserExchangeId == userId).ToList();
		}
	}
}
