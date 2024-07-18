using BusinessLayer.RequestModels;
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
    public class ExchangedService : IExchangedService
    {
        private readonly IUnitOfWork _unitOfWork;

        public ExchangedService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddExchangedAsync(Exchanged exchanged)
        {
            await _unitOfWork.Repository<Exchanged>().InsertAsync(exchanged);
            await _unitOfWork.CommitAsync();
        }

        public async Task AddExchangedProductAsync(ExchangedProduct exchangedProduct)
        {
            try
            {
                var product = await _unitOfWork.Repository<Product>().FindAsync(p => p.Id == exchangedProduct.ProductId && p.Status == true && p.IsForSell == false);
                if (product == null)
                {
                    throw new Exception("Product is either inactive or marked for sale.");
                }

                var existingPost = await _unitOfWork.Repository<Post>().FindAsync(p => p.ProductId == exchangedProduct.ProductId);
                if (existingPost != null)
                {
                    throw new Exception("Product is already part of an post.");
                }

                var existingExchangeProduct = await _unitOfWork.Repository<ExchangedProduct>().FindAsync(ep => ep.ProductId == exchangedProduct.ProductId);
                if (existingExchangeProduct != null)
                {
                    throw new Exception("Product is part of an exchange.");
                }

                await _unitOfWork.Repository<ExchangedProduct>().InsertAsync(exchangedProduct);

                product.Status = false;
                await _unitOfWork.Repository<Product>().Update(product, product.Id);

                await _unitOfWork.CommitAsync();
            }
            catch (Exception ex)
            {
                throw new Exception($"An error occurred while adding the exchanged product: {ex.Message}", ex);
            }
        }

        public async Task<IEnumerable<ExchangedResponseModel>> GetAllFinishedExchangedByUserIdAsync(int userId)
        {
            var exchangeds =  await _unitOfWork.Repository<Exchanged>()
                .GetAll()
                .Where(e => e.Status && (e.UserId == userId))
                .Include(e => e.User) 
                .Include(e => e.Post)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.User)
                .Include(e => e.Post.Ratings)
                .Include(e => e.ExchangedProducts)
                .ThenInclude(ep => ep.Product)
                .ToListAsync();

            var exchangedResponseModels = exchangeds.Select(exchanged => new ExchangedResponseModel
            {
                Id = exchanged.Id,
                Description = exchanged.Description,
                Date = exchanged.Date,
                Status = (bool)exchanged.Status,
                IsCompleted = (bool)exchanged.StatusRating,
                IsRated = exchanged.Post.Ratings.Any(r => r.PostId == exchanged.Post.Id),
                User = new UserResponse
                {
                    Id = exchanged.User.Id,
                    UserName = exchanged.User.UserName,
                    ImgUrl = exchanged.User.ImgUrl,
                },
                PostOwner = new UserResponse
                {
                    Id = exchanged.Post.User.Id,
                    UserName = exchanged.Post.User.UserName,
                    ImgUrl = exchanged.Post.User.ImgUrl,
                },
                Post = new PostResponse
                {
                    Id = exchanged.PostId,
                    Title = exchanged.Post.Title,
                    ImageUrl = exchanged.Post.ImageUrl
                },
                ProductOfPost = new ProductResponse
                {
                    Id = exchanged.Post.Product.Id,
                    Name = exchanged.Post.Product.Name,
                    UrlImg = exchanged.Post.Product.UrlImg
                },
                ExchangedProducts = exchanged.ExchangedProducts.Select(ep => new ProductResponseForExchange
                {
                    Id = ep.Product.Id,
                    Name = ep.Product.Name,
                    Description = ep.Product.Description,
                    UrlImg = ep.Product.UrlImg
                }).ToList()
            }).ToList();

            return exchangedResponseModels;
        }

        public async Task<IEnumerable<ExchangedResponseModel>> GetAllPendingExchangedByUserIdForCustomerAsync(int userId)
        {
            var exchangeds = await _unitOfWork.Repository<Exchanged>()
                .GetAll() 
                .Where(e => e.UserId == userId && !e.Status)
                .Include(e => e.User)
                .Include(e => e.Post)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.User)
                .Include(e => e.ExchangedProducts)
                .ThenInclude(ep => ep.Product)
                .ToListAsync();

            var exchangedResponseModels = exchangeds.Select(exchanged => new ExchangedResponseModel
            {
                Id = exchanged.Id,
                Description = exchanged.Description,
                Date = exchanged.Date,
                Status = (bool)exchanged.Status,
                User = new UserResponse
                {
                    Id = exchanged.User.Id,
                    UserName = exchanged.User.UserName,
                    ImgUrl = exchanged.User.ImgUrl,
                },
                PostOwner = new UserResponse
                {
                    Id = exchanged.Post.User.Id,
                    UserName = exchanged.Post.User.UserName,
                    ImgUrl = exchanged.Post.User.ImgUrl,
                },
                Post = new PostResponse
                {
                    Id = exchanged.PostId,
                    Title = exchanged.Post.Title,
                    ImageUrl = exchanged.Post.ImageUrl
                },
                ProductOfPost = new ProductResponse
                {
                    Id = exchanged.Post.Product.Id,
                    Name = exchanged.Post.Product.Name,
                    UrlImg = exchanged.Post.Product.UrlImg
                },
                ExchangedProducts = exchanged.ExchangedProducts.Select(ep => new ProductResponseForExchange
                {
                    Id = ep.Product.Id,
                    Name = ep.Product.Name,
                    Description = ep.Product.Description,
                    UrlImg = ep.Product.UrlImg
                }).ToList()
            }).ToList();

            return exchangedResponseModels;
        }

        public async Task<IEnumerable<ExchangedResponseModel>> GetAllPendingExchangedByUserIdForPosterAsync(int userId)
        {
            var exchangeds = await _unitOfWork.Repository<Exchanged>()
                .GetAll()
                .Include(e => e.User)
                .Include(e => e.Post)
                .ThenInclude(p => p.Product)
                .ThenInclude(p => p.User)
                .Where(e => e.Post.UserId == userId && !e.Status)
                .Include(e => e.ExchangedProducts)
                .ThenInclude(ep => ep.Product)
                .ToListAsync();

            var exchangedResponseModels = exchangeds.Select(exchanged => new ExchangedResponseModel
            {
                Id = exchanged.Id,
                Description = exchanged.Description,
                Date = exchanged.Date,
                Status = (bool)exchanged.Status,
                User = new UserResponse
                {
                    Id = exchanged.User.Id,
                    UserName = exchanged.User.UserName,
                    ImgUrl = exchanged.User.ImgUrl,
                },
                PostOwner = new UserResponse
                {
                    Id = exchanged.Post.User.Id,
                    UserName = exchanged.Post.User.UserName,
                    ImgUrl = exchanged.Post.User.ImgUrl,
                },
                Post = new PostResponse
                {
                    Id = exchanged.PostId,
                    Title = exchanged.Post.Title,
                    ImageUrl = exchanged.Post.ImageUrl
                },
                ProductOfPost = new ProductResponse
                {
                    Id = exchanged.Post.Product.Id,
                    Name = exchanged.Post.Product.Name,
                    UrlImg = exchanged.Post.Product.UrlImg
                },
                ExchangedProducts = exchanged.ExchangedProducts.Select(ep => new ProductResponseForExchange
                {
                    Id = ep.Product.Id,
                    Name = ep.Product.Name,
                    Description = ep.Product.Description,
                    UrlImg = ep.Product.UrlImg
                }).ToList()
            }).ToList();

            return exchangedResponseModels;
        }

        public async Task<Exchanged> GetExchangedByIdAsync(int id)
        {
            return await _unitOfWork.Repository<Exchanged>().GetById(id);
        }

        public async Task UpdateExchangedStatusAcceptAsync(int id)
        {
            var exchanged = await _unitOfWork.Repository<Exchanged>().GetById(id);
            if (exchanged == null)
            {
                throw new ArgumentException($"Exchanged with id {id} not found.");
            }

            // Update exchanged status
            exchanged.Status = true;
            exchanged.StatusRating = false;

            // Get the associated post
            var post = await _unitOfWork.Repository<Post>().GetById(exchanged.PostId);
            if (post == null)
            {
                throw new ArgumentException($"Post with id {exchanged.PostId} not found for exchanged id {id}.");
            }

            // Update post statuses
            post.PublicStatus = false;
            post.ExchangedStatus = true;

            // Update entities and commit changes
            await _unitOfWork.Repository<Post>().Update(post, post.Id);
            await _unitOfWork.Repository<Exchanged>().Update(exchanged, id);

            // Update the product in the post to false
            var postProduct = await _unitOfWork.Repository<Product>().GetById(post.ProductId);
            if (postProduct != null)
            {
                postProduct.Status = false;
                await _unitOfWork.Repository<Product>().Update(postProduct, postProduct.Id);
            }

            // Delete related exchanges except the one being accepted
            var exchangesToDelete = await _unitOfWork.Repository<Exchanged>()
                .GetAll()
                .Where(e => e.PostId == exchanged.PostId && e.Id != id)
                .ToListAsync();

            foreach (var exchange in exchangesToDelete)
            {
                var exchangedProductsToDelete = await _unitOfWork.Repository<ExchangedProduct>()
                .GetAll()
                .Where(ep => ep.ExchangeId == exchange.Id)
                .ToListAsync();

                foreach (var exchangedProduct in exchangedProductsToDelete)
                {
                    var product = await _unitOfWork.Repository<Product>().GetById(exchangedProduct.ProductId);
                    if (product != null)
                    {
                        product.Status = true;
                        await _unitOfWork.Repository<Product>().Update(product, product.Id);
                    }
                    await _unitOfWork.Repository<ExchangedProduct>().HardDelete(exchangedProduct.Id);
                }

                await _unitOfWork.Repository<Exchanged>().HardDelete(exchange.Id);
            }

            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateExchangedStatusDenyAsync(int id)
        {
            var exchangedProductsToDelete = await _unitOfWork.Repository<ExchangedProduct>()
                .GetAll()
                .Where(ep => ep.ExchangeId == id)
                .ToListAsync();

            foreach (var exchangedProduct in exchangedProductsToDelete)
            {
                var product = await _unitOfWork.Repository<Product>().GetById(exchangedProduct.ProductId);
                if (product != null)
                {
                    product.Status = true;
                    await _unitOfWork.Repository<Product>().Update(product, product.Id);
                }
                await _unitOfWork.Repository<ExchangedProduct>().HardDelete(exchangedProduct.Id);
            }

            // Delete the exchanged entity itself
            await _unitOfWork.Repository<Exchanged>().HardDelete(id);

            // Commit changes
            await _unitOfWork.CommitAsync();
        }

        public async Task CancelExchaneAsync(int id)
        {
            var exchangedProductsToDelete = await _unitOfWork.Repository<ExchangedProduct>()
                .GetAll()
                .Where(ep => ep.ExchangeId == id)
                .ToListAsync();

            foreach (var exchangedProduct in exchangedProductsToDelete)
            {
                var product = await _unitOfWork.Repository<Product>().GetById(exchangedProduct.ProductId);
                if (product != null)
                {
                    product.Status = true;
                    await _unitOfWork.Repository<Product>().Update(product, product.Id);
                }
                await _unitOfWork.Repository<ExchangedProduct>().HardDelete(exchangedProduct.Id);
            }

            // Delete the exchanged entity itself
            await _unitOfWork.Repository<Exchanged>().HardDelete(id);

            // Commit changes
            await _unitOfWork.CommitAsync();
        }

        public async Task UpdateExchangeStatusCompleted(int id)
        {
            var exchanged = await _unitOfWork.Repository<Exchanged>().GetById(id);
            if (exchanged == null)
            {
                throw new ArgumentException($"Exchanged with id {id} not found.");
            }

            exchanged.StatusRating = true;

            await _unitOfWork.Repository<Exchanged>().Update(exchanged, id);

            // Commit changes
            await _unitOfWork.CommitAsync();
        }

        public async Task DeleteExchangedAsync(int id)
        {
            await _unitOfWork.Repository<Exchanged>().HardDelete(id);
            await _unitOfWork.CommitAsync();
        }
    }
}
