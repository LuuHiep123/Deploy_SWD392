﻿using BusinessLayer.ResponseModels.Rating;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services
{
    public interface IRatingService
    {
        Task<RatingResponseModel> GetAll();
    }
}
