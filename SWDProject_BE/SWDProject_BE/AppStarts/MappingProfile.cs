using AutoMapper;
using BusinessLayer.RequestModels.Appeal;
using BusinessLayer.RequestModels.Category;
using BusinessLayer.RequestModels.Order;
using BusinessLayer.RequestModels.Rating;
using BusinessLayer.RequestModels.Report;
using BusinessLayer.RequestModels.Subcategory;
using BusinessLayer.ResponseModels;
using BusinessLayer.ResponseModels.Appeal;
using BusinessLayer.ResponseModels.Category;
using BusinessLayer.ResponseModels.Order;
using BusinessLayer.ResponseModels.Product;
using BusinessLayer.ResponseModels.Rating;
using BusinessLayer.ResponseModels.Report;
using BusinessLayer.ResponseModels.Subcategory;
using DataLayer.Model;

namespace SWDProject_BE.AppStarts
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, GetAllProductResponseModel>().ReverseMap();

            //Category
            CreateMap<SubcategoryResponseModel, SubCategory>().ReverseMap();
            CreateMap<CategoryRequestModel, Category>().ReverseMap();
            CreateMap<CategoryResponse, Category>().ReverseMap();


            //Subcategory
            CreateMap<SubCategoryRequestModel, SubCategory>().ReverseMap();
            CreateMap<SubcategoryResponseModel, SubCategory>().ReverseMap();


            //Report
            CreateMap<ReportRequestaUser, Report>().ReverseMap();
            CreateMap<ReportResponseModel, Report>().ReverseMap();

            //Rating
            CreateMap<RatingResponseModel, Rating>().ReverseMap();
            CreateMap<RatingRequestModel, Rating>().ReverseMap();
            CreateMap<RatingRequestModel, RatingResponseModel>().ReverseMap();

            //Appeal
            CreateMap<AppealResponseModel, Appeal>().ReverseMap();
            CreateMap<AddAppealRequestModel, Appeal>().ReverseMap();
            CreateMap<AppealResponseModel, AppealResponseModel>().ReverseMap();

            //Order
            CreateMap<AddOrderRequestModel, Order>().ReverseMap();
            CreateMap<OrderResponseModel, Order>().ReverseMap();

            //OrderDetails
            CreateMap<OrderDetailResponeModel, OrderDetail>().ReverseMap();

        }
    }
}
