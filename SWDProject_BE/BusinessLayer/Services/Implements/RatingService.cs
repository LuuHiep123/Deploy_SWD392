using AutoMapper;
using BusinessLayer.RequestModels.Rating;
using BusinessLayer.ResponseModels.Rating;
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
    public class RatingService : IRatingService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IPostService _postService;
        private readonly IUsersService _userService;
        private IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper, IPostService postService, IUsersService usersService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _postService = postService;
            _userService = usersService;
        }

        public async Task<bool> GetRatingByUser(int userId, int postId)
        {
            try
            {
                var ratingList = await _unitOfWork.Repository<Rating>().GetAll().Where(r => r.UserId == userId).ToListAsync();

                if (ratingList.Any())
                {
                    foreach (var rating in ratingList)
                    {
                        if (rating.PostId == postId)
                        {
                            return false;
                        }
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<bool> RatingPost(int userId ,RatingRequestModel dto)
        {
            try
            {
                if (await this.GetRatingByUser(userId, dto.PostId))
                {
                    var rating = _mapper.Map<Rating>(dto);
                    rating.UserId = userId;
                    rating.Status = true; 
                    rating.Date = DateTime.Now;
                    await _unitOfWork.Repository<Rating>().InsertAsync(rating);

                    var post = await _postService.GetPostByIdAsync(dto.PostId);
                    var userPost = await _userService.GetUserByIdAsync(post.UserId);
                    
                    if (userId == post.UserId)
                    {
                        throw new Exception("You can't rating your own Post");
                    }

                    if (userPost.RatingCount == null)
                    {
                        int total = dto.Score;
                        userPost.RatingCount = total;
                        await _userService.UpdateUserAsync(userPost);
                    }
                    else
                    {
                        int total = (int)userPost.RatingCount + dto.Score;
                        userPost.RatingCount = total;
                        await _userService.UpdateUserAsync(userPost);
                    }
                    return true;                  
                }
                return false;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
    }
}
