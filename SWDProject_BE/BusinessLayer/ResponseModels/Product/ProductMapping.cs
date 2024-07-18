using AutoMapper;
using DataLayer.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.ResponseModels.Product
{
    public static class ProductMapping
    {
        public static GetAllProductResponseModel MapToGetAllProduct(this DataLayer.Model.Product projectFrom, IMapper mapper)
            => mapper.Map<GetAllProductResponseModel>(projectFrom);

        public static List<GetAllProductResponseModel> MapToGetAllProductList(this IEnumerable<DataLayer.Model.Product> projectFrom, IMapper mapper)
            => projectFrom.Select(x => x.MapToGetAllProduct(mapper)).ToList();


    }
}
