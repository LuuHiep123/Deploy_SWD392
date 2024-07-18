using BusinessLayer.RequestModels.Product;
using BusinessLayer.ResponseModels.Product;
using DataLayer.Dto.Product;
using DataLayer.Model;
using DataLayer.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IProductService
    {

        Task<List<GetAllProductResponseModel>> GetAllProductsValid();
        Task<List<GetAllProductResponseModel>> GetAllProducts();
        Task<List<GetAllProductResponseModel>> GetAllProductsForExchangeByUserId(int userId);
        Task<String> addProduct(AddProductDto dto, int userId);
        Task<String> addProductForExchange(AddProductDto dto, int userId);

        Task<String> updateProduct(int id, UpdateProductDto dto);

        Task<String> deleteProduct(int id);

        Task<GetAllProductResponseModel> GetProductDetailsResponseModel(int id);
        Task<List<GetAllProductResponseModel>> GetProductByUserId(int userId);
        Task<List<GetAllProductResponseModel>> GetAllProductsForExchane();

    }
}
