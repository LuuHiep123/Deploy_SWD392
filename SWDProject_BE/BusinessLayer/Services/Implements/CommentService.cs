using BusinessLayer.ResponseModels;
using DataLayer.Model;
using DataLayer.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Implements
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;

        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<CommentResponseModel>> GetAllCommentsByPostAsync(int postId)
        {
            var comments = await _unitOfWork.Repository<Comment>()
                .GetAll()
                .Include(c => c.User)
                .Where(c => c.PostId == postId)
                .ToListAsync();

            var commentResponseModels = comments.Select(comment => new CommentResponseModel
            {
                id = comment.Id,
                PostId = comment.PostId,
                Content = comment.Content,
                Status = (bool)comment.Status,
                Date = comment.Date,
                User = new UserResponse
                {
                    Id = comment.User.Id,
                    UserName = comment.User.UserName,
                    ImgUrl = comment.User.ImgUrl,
                },
            }).ToList();

            return commentResponseModels;
        }

        public async Task<Comment> GetCommentByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Comment>().GetById(id);
        }

        public async Task AddCommentAsync(Comment comment)
        {
            await _unitOfWork.Repository<Comment>().InsertAsync(comment);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateCommentAsync(Comment comment)
        {
            await _unitOfWork.Repository<Comment>().Update(comment, comment.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            await _unitOfWork.Repository<Comment>().HardDelete(id);
            await _unitOfWork.CommitAsync();
        }
	}
}
