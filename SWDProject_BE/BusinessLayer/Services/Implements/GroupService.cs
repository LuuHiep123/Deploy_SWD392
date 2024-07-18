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

		public async Task<List<Group>> FindAllByUserId(int userId)
		{
			var user = await _unitOfWork.Repository<User>().FindAsync(u => u.Id == userId);
			if (user == null)
			{
				return null;
			}
			return await _unitOfWork.Repository<Group>().GetAll().Include(p => p.Post).Where(g => g.UserExchangeId == userId || g.Post.UserId == userId).ToListAsync();

        }
	}
}
