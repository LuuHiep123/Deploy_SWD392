using BusinessLayer.ResponseModels;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
	public interface IUsersService
	{
		IEnumerable<User> GetUsers();
		Task<User> GetUserByIdAsync(int id);
		Task CreateUserAsync(User user);
		Task UpdateUserAsync(User user);
		Task DeleteUserAsync(int id);
        Task<bool> UserExistsAsync(int id);
		Task<User> GetUserByUsernameAsync(string username);
		Task<User> GetUserByEmailAsync(string email);
		Task<UserDetailResponse> GetUserProfile(int id);
    }

}
