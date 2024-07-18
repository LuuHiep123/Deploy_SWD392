using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using BusinessLayer.ResponseModels;

namespace BusinessLayer.Services.Implements
{
	public class UsersServices : IUsersService
	{
		private readonly IUnitOfWork _unitOfWork;

		public UsersServices(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}

		public IEnumerable<User> GetUsers()
		{
			return _unitOfWork.Repository<User>().GetAll();

		}

		public async Task<User> GetUserByIdAsync(int id)
		{
			return await _unitOfWork.Repository<User>().GetById(id);
		}

		public async Task CreateUserAsync(User user)
		{
			await _unitOfWork.Repository<User>().InsertAsync(user);
			await _unitOfWork.CommitAsync();
		}

		public async Task UpdateUserAsync(User user)
		{
			await _unitOfWork.Repository<User>().Update(user, user.Id);
			await _unitOfWork.CommitAsync();
		}

		public async Task DeleteUserAsync(int id)
		{
			var user = await _unitOfWork.Repository<User>().GetById(id);
			if (user != null)
			{
				_unitOfWork.Repository<User>().Delete(user);
				await _unitOfWork.CommitAsync();
			}
		}

        public async Task<bool> UserExistsAsync(int id)
		{
			var user = await _unitOfWork.Repository<User>().GetById(id);
			return user != null;
		}

		public async Task<User> GetUserByUsernameAsync(string username)
		{
			// Truy vấn người dùng từ cơ sở dữ liệu theo tên người dùng
			var user = await _unitOfWork.Repository<User>()
				.GetAll()
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.UserName == username);

			return user;
		}

		public async Task<User> GetUserByEmailAsync(string email)
		{
			// Truy vấn người dùng từ cơ sở dữ liệu theo tên người dùng
			var user = await _unitOfWork.Repository<User>()
				.FindAsync(predicate: u => u.Email == email);

			return user;
		}

        public async Task<UserDetailResponse> GetUserProfile(int id)
        {
            var user = await _unitOfWork.Repository<User>().GetById(id);

            if (user == null)
            {
                throw new Exception($"User with ID {id} not found.");
            }
            var ratings = await _unitOfWork.Repository<Rating>()
                                                .GetAll()
												.Include(r => r.Post)
                                                .Where(r => r.Post.UserId == id)
                                                .ToListAsync();
            double averageRating = 0;
            if (ratings.Any())
            {
                averageRating = ratings.Average(r => r.Score);
            }

            var responseModel = new UserDetailResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                Dob = user.Dob,
                Address = user.Address,
                PhoneNumber = user.PhoneNumber,
                RoleId = user.RoleId,
                ImgUrl = user.ImgUrl,
                Gender = user.Gender,
				RatingCount = averageRating,
            };

            return responseModel;
        }
    }

}
