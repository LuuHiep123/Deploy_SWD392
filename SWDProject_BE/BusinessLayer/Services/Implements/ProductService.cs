using DataLayer.Dto.Product;
using DataLayer.Model;
using DataLayer.Repository;
using Microsoft.EntityFrameworkCore;
using BusinessLayer.Services.Implements;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataLayer.UnitOfWork;
using BusinessLayer.RequestModels.Product;
using BusinessLayer.ResponseModels.Product;
using AutoMapper;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using BusinessLayer.ResponseModels.Category;

namespace BusinessLayer.Services
{
    public class ProductService : IProductService
    {
        private IUnitOfWork unitOfWork;
        private IPostService _postService;
        public IMapper _mapper;

        public ProductService(IUnitOfWork unitOfWork, IMapper _mapper, IPostService postService)
        {
            _postService = postService;
            this.unitOfWork = unitOfWork;
            this._mapper = _mapper;
        }

        public async Task<String> addProduct(AddProductDto dto, int userId)
        {
            try
            {
                Product product = new Product();
                product.UserId = userId;
                product.SubCategoryId = dto.SubcategoryId;
                product.CategoryId = dto.CategoryId;
                product.Name = dto.Name;
                product.Price = dto.Price;
                product.Description = dto.Description;
                product.Location = dto.Location;
                product.UrlImg = dto.UrlImg;
                product.Status = true;
                product.IsForSell = true;
                await unitOfWork.Repository<Product>().InsertAsync(product);
                await unitOfWork.CommitAsync();
                return "add product successfull!";
            }
            catch (Exception ex)
            {
                throw;
            }   
        }

        public async Task<String> addProductForExchange(AddProductDto dto, int userId)
        {
            try
            {
                Product product = new Product();
                product.UserId = userId;
                product.SubCategoryId = dto.SubcategoryId;
                product.CategoryId = dto.CategoryId;
                product.Name = dto.Name;
                product.Price = dto.Price;
                product.Description = dto.Description;
                product.Location = dto.Location;
                product.UrlImg = dto.UrlImg;
                product.Status = true;
                product.IsForSell = false;
                await unitOfWork.Repository<Product>().InsertAsync(product);
                await unitOfWork.CommitAsync();
                return "add product successfull!";
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> deleteProduct(int id)
        {
            try
            {
                var product = await unitOfWork.Repository<Product>().GetAll()
                        .Include(p => p.ExchangedProducts)
                        .Include(p => p.Posts)
                        .FirstOrDefaultAsync(p => p.Id == id);

                if (product != null)
                {
                    if (product.Posts.Any())
                    {
                        throw new Exception("Cannot delete product as it is already in a post.");
                    }

                    if (product.ExchangedProducts.Any())
                    {
                        throw new Exception("Cannot delete product as it is already in an exchange.");
                    }

                    product.Status = false;    
                    await unitOfWork.Repository<Product>().Update(product, id);
                    await unitOfWork.CommitAsync();
                    return "Delete Successfull";
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<GetAllProductResponseModel>> GetAllProductsValid()
        {
            try
            {
                var products = await unitOfWork.Repository<Product>()
                    .GetAll()
                    .Where(p => p.Status == true && p.IsForSell == true)
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.SubCategory)
                    .ToListAsync();

                List<GetAllProductResponseModel> final = products.Select(product =>
                {
                    var result = product.MapToGetAllProduct(_mapper);
                    result.UserName = product.User.UserName;
                    result.CategoryName = product.Category.Name;
                    result.SubcategoryName = product.SubCategory.Name;
                    return result;
                }).ToList();

                return final;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetAllProductResponseModel>> GetAllProducts()
        {
            try
            {
                var Product = await unitOfWork.Repository<Product>().GetAll().ToListAsync();
                List<GetAllProductResponseModel> Final = new List<GetAllProductResponseModel>();
                foreach (var product in Product)
                {
                    var user = await unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(product.UserId));
                    var category = await unitOfWork.Repository<Category>().FindAsync(c => c.Id.Equals(product.CategoryId));
                    var Subcategory = await unitOfWork.Repository<SubCategory>().FindAsync(c => c.Id.Equals(product.SubCategoryId));
                    GetAllProductResponseModel result = new GetAllProductResponseModel();
                    result = product.MapToGetAllProduct(_mapper);
                    result.UserName = user.UserName;
                    result.CategoryName = category.Name;
                    result.SubcategoryName = Subcategory.Name;
                    Final.Add(result);
                }
                return Final;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<string> updateProduct(int id,UpdateProductDto dto)
        {
            try
            {
                var product = await unitOfWork.Repository<Product>().GetById(id);
                if (product != null)
                {
                    var check = await _postService.GetPostByProductId(id);
                    if (check == null)
                    {
                        if (dto.CategoryId != 0)
                        {
                            product.CategoryId = dto.CategoryId.Value;
                        }
                        if (dto.SubcategoryId != 0)
                        {
                            product.SubCategoryId = dto.SubcategoryId.Value;
                        }
                        if (dto.Name != null)
                        {
                            product.Name = dto.Name;
                        }
                        if (dto.Price != 0)
                        {
                            product.Price = (double)dto.Price;
                        }
                        if (dto.Description != null)
                        {
                            product.Description = dto.Description;
                        }
                        if (dto.UrlImg != null)
                        {
                            product.UrlImg = dto.UrlImg;
                        }
                        if (dto.Status.HasValue)
                        {
                            product.Status = dto.Status.Value;
                        }
                        await unitOfWork.Repository<Product>().Update(product, id);
                        await unitOfWork.CommitAsync();
                        return "Update Successfull";
                    }
                    else
                    {
                        return "Products already in the post can not be edited!";
                    }
                }
                else
                {
                    return "Product not fount!";
                }
            }
            catch(Exception ex)
            {
                throw;
            }    
        }

        public async Task<GetAllProductResponseModel> GetProductDetailsResponseModel(int id)
        {
            try
            {
                var product = await unitOfWork.Repository<Product>().GetById(id);
                if (product != null)
                {
                    var user = await unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(product.UserId));
                    var category = await unitOfWork.Repository<Category>().FindAsync(c => c.Id.Equals(product.CategoryId));
                    var Subcategory = await unitOfWork.Repository<SubCategory>().FindAsync(c => c.Id.Equals(product.SubCategoryId));
                    GetAllProductResponseModel model = new GetAllProductResponseModel();
                    model = product.MapToGetAllProduct(_mapper);
                    model.UserName = user.UserName;
                    model.UserImgUrl = user.ImgUrl;
                    return model;
                }
                return null;
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetAllProductResponseModel>> GetProductByUserId(int userId)
        {
            try
            {
                var listProduct = await unitOfWork.Repository<Product>()
                    .GetAll()
                    .Where(p => p.Status == true && p.UserId == userId && p.IsForSell == true)
                    .Include(p => p.User)
                    .Include(p => p.Category)
                    .Include(p => p.SubCategory)
                    .ToListAsync();

                if (listProduct != null && listProduct.Any())
                {
                    List<GetAllProductResponseModel> final = listProduct.Select(product =>
                    {
                        var result = product.MapToGetAllProduct(_mapper);
                        result.UserName = product.User.UserName;
                        result.CategoryName = product.Category.Name;
                        result.SubcategoryName = product.SubCategory.Name;
                        return result;
                    }).ToList();

                    return final;
                }
                else
                {
                    return new List<GetAllProductResponseModel>();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<List<GetAllProductResponseModel>> GetAllProductsForExchangeByUserId(int userId)
        {
            try
            {
                var Product = await unitOfWork.Repository<Product>().GetAll().Where(p => p.Status == true && p.IsForSell == false && p.UserId == userId).ToListAsync();
                List<GetAllProductResponseModel> Final = new List<GetAllProductResponseModel>();
                foreach (var product in Product)
                {
                    var post = await unitOfWork.Repository<Post>().FindAsync(p => p.ProductId == product.Id);
                    if (post == null)
                    {
                        var user = await unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(product.UserId));
                        var category = await unitOfWork.Repository<Category>().FindAsync(c => c.Id.Equals(product.CategoryId));
                        var Subcategory = await unitOfWork.Repository<SubCategory>().FindAsync(c => c.Id.Equals(product.SubCategoryId));
                        GetAllProductResponseModel result = new GetAllProductResponseModel();
                        result = product.MapToGetAllProduct(_mapper);
                        result.UserName = user.UserName;
                        result.CategoryName = category.Name;
                        result.SubcategoryName = Subcategory.Name;
                        Final.Add(result);
                    }
                }
                return Final;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<GetAllProductResponseModel>> GetAllProductsForExchane()
        {
            try
            {
                List<GetAllProductResponseModel> Final = new List<GetAllProductResponseModel>();

                var Product = await unitOfWork.Repository<Product>().GetAll().Where(p => p.Status == true && p.IsForSell == false).ToListAsync();

                foreach (var product in Product)
                {
                    var user = await unitOfWork.Repository<User>().FindAsync(u => u.Id.Equals(product.UserId));
                    var category = await unitOfWork.Repository<Category>().FindAsync(c => c.Id.Equals(product.CategoryId));
                    var Subcategory = await unitOfWork.Repository<SubCategory>().FindAsync(c => c.Id.Equals(product.SubCategoryId));
                    GetAllProductResponseModel result = new GetAllProductResponseModel();
                    result = product.MapToGetAllProduct(_mapper);
                    result.UserName = user.UserName;
                    result.CategoryName = category.Name;
                    result.SubcategoryName = Subcategory.Name;
                    Final.Add(result);
                }
                return Final;

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
