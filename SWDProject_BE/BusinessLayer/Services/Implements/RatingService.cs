using AutoMapper;
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
        private IMapper _mapper;

        public RatingService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<RatingResponseModel> GetAll()
        {
            try
            {
                var rating = await _unitOfWork.Repository<Rating>().GetAll().ToListAsync();
                var result = _mapper.Map<RatingResponseModel>(rating);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
