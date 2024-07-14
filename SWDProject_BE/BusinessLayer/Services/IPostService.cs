using BusinessLayer.ResponseModels;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IPostService
    {
        Task<IEnumerable<PostResponseModel>> GetAllValidPostsAsync();
        Task<IEnumerable<PostResponseModel>> GetAllUnpublicPostsAsync();
        Task<IEnumerable<PostResponseModelByUser>> GetAllPostsByUserIdAsync(int userId);
        Task<Post> GetPostByIdAsync(int id);
        Task<PostDetailResponseModel> GetPostDetailAsync(int id, int userId);
        Task AddPostAsync(Post post);
        Task UpdatePostAsync(Post post);
        Task UpdatePostStatusAsync(int id, bool newPublicStatus);
        Task DeletePostAsync(int id);
    }
}
