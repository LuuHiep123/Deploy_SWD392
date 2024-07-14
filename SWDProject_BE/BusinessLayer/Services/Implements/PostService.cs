using BusinessLayer.ResponseModels;
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
    public class PostService : IPostService
    {
        private readonly IUnitOfWork _unitOfWork;

        public PostService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<PostResponseModel>> GetAllValidPostsAsync()
        {
            var posts = await _unitOfWork.Repository<Post>()
                            .GetAll()
                            .Where(p => p.PublicStatus == true && p.ExchangedStatus == false)
                            .Include(p => p.User)
                            .Include(p => p.Product)
                            .ToListAsync();

            var postResponseModels = posts.Select(post => new PostResponseModel
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Date = post.Date,
                PublicStatus = (bool)post.PublicStatus,
                ImageUrl = post.ImageUrl,
                User = new UserResponse
                {
                    Id = post.User.Id,
                    UserName = post.User.UserName,
                    ImgUrl = post.User.ImgUrl,
                },
                Product = new ProductResponse
                {
                    Id = post.Product.Id,
                    Name = post.Product.Name,
                    UrlImg = post.Product.UrlImg
                }
            }).ToList();

            return postResponseModels;
        }

        public async Task<IEnumerable<PostResponseModel>> GetAllUnpublicPostsAsync()
        {
            var posts = await _unitOfWork.Repository<Post>()
                            .GetAll()
                            .Where(p => p.PublicStatus == false &&  p.ExchangedStatus == false)
                            .Include(p => p.User)
                            .Include(p => p.Product)
                            .ToListAsync();

            var postResponseModels = posts.Select(post => new PostResponseModel
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Date = post.Date,
                PublicStatus = (bool)post.PublicStatus,
                ImageUrl = post.ImageUrl,
                User = new UserResponse
                {
                    Id = post.User.Id,
                    UserName = post.User.UserName,
                    ImgUrl = post.User.ImgUrl,
                },
                Product = new ProductResponse
                {
                    Id = post.Product.Id,
                    Name = post.Product.Name,
                    UrlImg = post.Product.UrlImg
                }
            }).ToList();

            return postResponseModels;
        }

        public async Task<IEnumerable<PostResponseModelByUser>> GetAllPostsByUserIdAsync(int userId)
        {
            var posts = await _unitOfWork.Repository<Post>()
                                 .GetAll()
                                 .Where(p => p.UserId == userId)
                                 .Include(p => p.User)
                                 .Include(p => p.Reports)
                                 .Include(p => p.Product)
                                 .ToListAsync();

            var postResponseModels = posts.Select(post => new PostResponseModelByUser
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Date = post.Date,
                PublicStatus = (bool)post.PublicStatus,
                ImageUrl = post.ImageUrl,
                isExchanged = (bool)post.ExchangedStatus,
                isReported = post.Reports.Any(r => r.Status == true && post.PublicStatus == false),
                User = new UserResponse
                {
                    Id = post.User.Id,
                    UserName = post.User.UserName,
                    ImgUrl = post.User.ImgUrl,
                },
                Product = new ProductResponse
                {
                    Id = post.Product.Id,
                    Name = post.Product.Name,
                    UrlImg = post.Product.UrlImg
                }
            }).ToList();

            return postResponseModels;
        }

        public async Task<Post> GetPostByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Post>().GetById(id);
        }

        public async Task<PostDetailResponseModel> GetPostDetailAsync(int id, int userId)
        {
            var post = await _unitOfWork.Repository<Post>().ObjectMapper(
                selector: p => new
                {
                    p.Id,
                    p.Title,
                    p.Description,
                    p.Date,
                    p.PublicStatus,
                    p.ImageUrl,
                    User = p.User == null ? null : new
                    {
                        p.User.Id,
                        p.User.UserName,
                        p.User.ImgUrl
                    },
                    Product = p.Product == null ? null : new
                    {
                        p.Product.Id,
                        p.Product.Name,
                        p.Product.UrlImg,
                        Exchanged = p.Exchangeds.FirstOrDefault(e => e.UserId == userId) == null ? null : new
                        {
                            ExchangeId = p.Exchangeds.FirstOrDefault().Id,
                        }
                    },
                    isReported = p.Reports.Any(r => r.Status == true && p.PublicStatus == false)
                },
                predicate: p => p.Id == id,
                include: query => query.Include(p => p.User)
                .Include(p => p.Product)
                .Include(p => p.Reports)
                .Include(p => p.Exchangeds)
            ).FirstOrDefaultAsync();



            var postResponseModel = new PostDetailResponseModel()
            {
                Id = post.Id,
                Title = post.Title,
                Description = post.Description,
                Date = post.Date,
                PublicStatus = (bool)post.PublicStatus,
                ImageUrl = post.ImageUrl,
                User = new UserResponse
                {
                    Id = post.User.Id,
                    UserName = post.User.UserName,
                    ImgUrl = post.User.ImgUrl,
                },
                Product = new ProductResponse
                {
                    Id = post.Product.Id,
                    Name = post.Product.Name,
                    UrlImg = post.Product.UrlImg
                },
                isReported = post.isReported,
                ExchangeId = post.Product.Exchanged?.ExchangeId,
                IsExchangedByUser = post.Product.Exchanged != null
            };

            return postResponseModel;
        }
        public async Task AddPostAsync(Post post)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().FindAsync(p => p.Id == post.ProductId && p.Status == true && p.IsForSell == false);
                if (product == null)
                {
                    throw new Exception("Product is either inactive or marked for sale.");
                }

                var existingPost = await _unitOfWork.Repository<Post>().FindAsync(p => p.ProductId == post.ProductId);
                if (existingPost != null)
                {
                    throw new Exception("Product is already part of an post.");
                }

                var exchangeProduct = await _unitOfWork.Repository<ExchangedProduct>().FindAsync(ep => ep.ProductId == post.ProductId);
                if (exchangeProduct != null)
                {
                    throw new Exception("Product is part of an exchange.");
                }

                await _unitOfWork.Repository<Post>().InsertAsync(post);
                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the post: {ex.Message}", ex);
            }
        }

        public async Task UpdatePostAsync(Post post)
        {
            await _unitOfWork.Repository<Post>().Update(post, post.Id);
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdatePostStatusAsync(int id, bool newPublicStatus)
        {
            var post = await _unitOfWork.Repository<Post>().GetById(id);
            if (post != null)
            {
                post.PublicStatus = newPublicStatus;

                await _unitOfWork.Repository<Post>().Update(post, post.Id);

                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("Post not found");
            }
        }

        public async Task DeletePostAsync(int id)
        {
            var post = await _unitOfWork.Repository<Post>()
                                .GetAll()
                                .Include(p => p.Reports)
                                .FirstOrDefaultAsync(p => p.Id == id);


            if (post != null)
            {
                var reports = post.Reports.ToList();
                foreach (var report in reports)
                {
                    await _unitOfWork.Repository<Report>().HardDelete(report.Id);
                }
                await _unitOfWork.CommitAsync();

                await _unitOfWork.Repository<Post>().HardDelete(id);
                await _unitOfWork.CommitAsync();
            }
            else
            {
                throw new Exception("Post not found");
            }
        }
    }
}
